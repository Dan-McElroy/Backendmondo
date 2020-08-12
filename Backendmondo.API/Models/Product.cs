using Backendmondo.API.Models.DTOs;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backendmondo.API.Models
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int DurationMonths { get; set; }

        public float PriceUSD { get; set; }

        public float TaxUSD { get; set; }

        public ProductDTO ToDTO()
        {
            return new ProductDTO
            {
                Name = Name,
                Duration = DurationMonths,
                Price = PriceUSD,
                Tax = TaxUSD
            };
        }
    }
}
