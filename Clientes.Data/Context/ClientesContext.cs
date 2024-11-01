using Microsoft.EntityFrameworkCore;
using Clientes.Data.Models;

namespace Clientes.Data.Context
{
    public class ClientesContext : DbContext
    {
        public ClientesContext(DbContextOptions<ClientesContext> options) : base(options) { }

        public DbSet<Models.Clientes> Clientes { get; set; }
    }
}
