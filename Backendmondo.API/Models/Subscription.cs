using Backendmondo.API.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Backendmondo.API.Models
{
    public class Subscription : ISoftDelete
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public User User { get; set; }

        public ICollection<ProductPurchase> ProductsPurchased { get; set; }

        public ICollection<SubscriptionPause> Pauses { get; set; }

        private bool IsPaused => Pauses.Any(pause => pause.IsOngoing);

        private DateTime? Purchased
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

        private int TotalDurationMonths => ProductsPurchased.Sum(purchase => purchase.Product.DurationMonths);

        private TimeSpan TotalPauseDuration =>
            TimeSpan.FromMilliseconds(Pauses
                .Where(pause => !pause.IsOngoing)
                .Sum(pause => pause.Duration.TotalMilliseconds));

        private DateTime? Expires
        {
            get
            {
                if (IsPaused || !Purchased.HasValue)
                {
                    return null;
                }
                var combinedProductDurationMonths = ProductsPurchased.Sum(purchase => purchase.Product.DurationMonths);
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
                TotalDuration = TotalDurationMonths,
                StartDate = Purchased?.ToString("yyyy-MM-dd"),
                EndDate = Expires?.ToString("yyyy-MM-dd"),
                PurchasedProducts = ProductsPurchased.Select(purchased => purchased.ToDTO()).OrderBy(purchase => purchase.DateOfPurchase)
            };
        }
    }
}
