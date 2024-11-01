using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Clientes.Abstractions;
using Clientes.Data.Context;
using Clientes.Data.Models;
using Clientes.Domain.DTO;

namespace Clientes.Services
{
    public class ClientesServices(IDbContextFactory<ClientesContext> DbFactory) : IClientesService
    {
        public async Task<ClientesDto> Buscar(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            var clientes = await contexto.Clientes
                .Where(e => e.ClienteId == id)
                .Select(p => new ClientesDto()
                {
                    ClienteId = p.ClienteId,
                    Nombres = p.Nombres,
                    WhatsApp = p.WhatsApp
                }).FirstOrDefaultAsync();

            return clientes;
        }

        public async Task<bool> Eliminar(int clienteId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Clientes
                .Where(e => e.ClienteId == clienteId)
                .ExecuteDeleteAsync() > 0;
        }

        public async Task<bool> ExisteCliente(int id, string nombres)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Clientes
                .AnyAsync(e => e.ClienteId != id && e.Nombres.ToLower().Equals(nombres.ToLower()));
        }

        private async Task<bool> Insertar(ClientesDto clientesDto)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            var cliente = new Data.Models.Clientes()
            {
                
                Nombres = clientesDto.Nombres,
                WhatsApp = clientesDto.WhatsApp
            };
            contexto.Clientes.Add(cliente);
            var guardo = await contexto.SaveChangesAsync() > 0;
            clientesDto.ClienteId = cliente.ClienteId;
            return guardo;
        }

        private async Task<bool> Modificar(ClientesDto clientesDto)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            var cliente = new Data.Models.Clientes()
            {
                ClienteId = clientesDto.ClienteId,
                Nombres = clientesDto.Nombres,
                WhatsApp = clientesDto.WhatsApp
            };
            contexto.Update(cliente);
            var modificado = await contexto.SaveChangesAsync() > 0;
            return modificado;
        }

        private async Task<bool> Existe(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Clientes 
                .AnyAsync(e => e.ClienteId == id);
        }

        public async Task<bool> Guardar(ClientesDto cliente)
        {
            if (!await Existe(cliente.ClienteId))
                return await Insertar(cliente);
            else
                return await Modificar(cliente);
        }

        public async Task<List<ClientesDto>> Listar(Expression<Func<ClientesDto, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Clientes.Select(p => new ClientesDto()
            {
                ClienteId = p.ClienteId,
                Nombres = p.Nombres,
                WhatsApp = p.WhatsApp,
            })
                .Where(criterio)
                .ToListAsync();
        }
    }

}
