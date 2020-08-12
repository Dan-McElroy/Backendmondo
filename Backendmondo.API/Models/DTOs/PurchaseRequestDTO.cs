using System.ComponentModel.DataAnnotations;

namespace Backendmondo.API.Models.DTOs
{
    public class PurchaseRequestDTO
    {
        public string ProductId { get; set; }

        [EmailAddress]
        public string UserEmail { get; set; }
    }
}
