using Backendmondo.API.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Backendmondo.Tests")]
namespace Backendmondo.API.Models
{

    public class Subscription : ISoftDelete
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public User User { get; set; }

        public ICollection<ProductPurchase> ProductsPurchased { get; set; }

        public ICollection<SubscriptionPause> Pauses { get; set; }

        public SubscriptionPause ActivePause => Pauses.FirstOrDefault(pause => pause.IsOngoing);

        internal DateTime? Purchased
        {
            get
            {
                if (!ProductsPurchased.Any())
                {
                    return null;
                }
                return ProductsPurchased.Min(purchase => purchase.Purchased);
            }
        }

        internal int TotalDurationMonths => ProductsPurchased.Sum(purchase => Math.Max(0, purchase.Product.DurationMonths));

        internal float TotalPurchaseCostUSD => ProductsPurchased.Sum(purchase => purchase.PriceUSDWhenPurchased);

        internal float TotalTaxCostUSD => ProductsPurchased.Sum(purchase => purchase.TaxUSDWhenPurchased);

        internal TimeSpan TotalPauseDuration =>
            TimeSpan.FromMilliseconds(Pauses
                .Where(pause => !pause.IsOngoing)
                .Sum(pause => Math.Max(0, pause.Duration.TotalMilliseconds)));

        internal DateTime? Expires
        {
            get
            {
                if (ActivePause != null || !Purchased.HasValue)
                {
                    return null;
                }
                var combinedProductDurationMonths = ProductsPurchased.Sum(purchase => Math.Max(0, purchase.Product.DurationMonths));
                return Purchased.Value.AddMonths(combinedProductDurationMonths) + TotalPauseDuration;
            }
        }

        public Subscription()
        {
            Pauses = new List<SubscriptionPause>();
            ProductsPurchased = new List<ProductPurchase>();
        }

        public Subscription(User user)
        : this()
        {
            User = user;
        }

        public SubscriptionDTO ToDTO()
        {
            return new SubscriptionDTO
            {
                Id = Id.ToString(),
                TotalDurationMonths = TotalDurationMonths,
                StartDate = Purchased?.ToString("yyyy-MM-dd"),
                EndDate = Expires?.ToString("yyyy-MM-dd"),
                PurchasedProducts = ProductsPurchased.Select(purchased => purchased.ToDTO()).OrderBy(purchase => purchase.DateOfPurchase),
                TotalPurchaseCostUSD = TotalPurchaseCostUSD,
                TotalTaxCostUSD = TotalTaxCostUSD,
                DatePaused = ActivePause?.Started.ToString("yyyy-MM-dd")
            };
        }
    }
}
