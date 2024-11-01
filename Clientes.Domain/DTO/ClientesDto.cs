using System.ComponentModel.DataAnnotations;

namespace Clientes.Domain.DTO
{
    public class ClientesDto

    {
      
        public int ClienteId { get; set; }

     
        public string? Nombres { get; set; }

        
        public string? WhatsApp { get; set; }
    }
}
