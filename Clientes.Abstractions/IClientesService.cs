using System.Linq.Expressions;
using Clientes.Domain.DTO;

namespace Clientes.Abstractions
{
    public interface IClientesService
    {
        Task<bool> Guardar(ClientesDto clientes);
        Task<bool> Eliminar(int clientesId);

        Task<ClientesDto> Buscar(int id);

        Task<List<ClientesDto>> Listar(Expression<Func<ClientesDto, bool>> criterio);

        Task<bool> ExisteCliente(int id, string nombres);
    }
}
