using Backendmondo.API.Models.DTOs;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backendmondo.API.Models
{
    public class ProductPurchase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Subscription Subscription { get; set; }

        public Product Product { get; set; }
        
        public DateTime Purchased { get; set; }

        public float PriceUSDWhenPurchased { get; private set; }

        public float TaxUSDWhenPurchased { get; private set; }

        public ProductPurchase() { }

        public ProductPurchase(Product product, Subscription subscription)
        {
            Product = product;
            Subscription = subscription;
            PriceUSDWhenPurchased = product.PriceUSD;
            TaxUSDWhenPurchased = product.TaxUSD;
            Purchased = DateTime.UtcNow;
        }

        public ProductPurchaseDTO ToDTO()
        {
            return new ProductPurchaseDTO
            {
                ProductId = Product.Id.ToString(),
                DurationMonths = Product.DurationMonths,
                Name = Product.Name,
                DateOfPurchase = Purchased.ToString("yyyy-MM-dd"),
                PriceUSDWhenPurchased = PriceUSDWhenPurchased,
                TaxUSDWhenPurchased = TaxUSDWhenPurchased,
                PriceUSD = Product.PriceUSD,
                TaxUSD = Product.TaxUSD,
            };
        }
    }
}
