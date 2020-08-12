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

        public ICollection<Product> Products { get; set; }

        public DateTime Purchased { get; set; }

        public ICollection<SubscriptionPause> Pauses { get; set; }

        private bool IsPaused => Pauses.Any(pause => pause.IsOngoing);

        private int TotalDurationMonths => Products.Sum(product => product.DurationMonths);

        private TimeSpan TotalPauseDuration =>
            TimeSpan.FromMilliseconds(Pauses
                .Where(pause => !pause.IsOngoing)
                .Sum(pause => pause.Duration.TotalMilliseconds));

        private DateTime? Expires
        {
            get
            {
                if (IsPaused)
                {
                    return null;
                }
                var combinedProductDurationMonths = Products.Sum(product => product.DurationMonths);
                return Purchased.AddMonths(combinedProductDurationMonths) + TotalPauseDuration;
            }
        }

        public Subscription()
        {
            Pauses = new List<SubscriptionPause>();
            Products = new List<Product>();
        }


        public SubscriptionDTO ToDTO()
        {
            return new SubscriptionDTO
            {
                Id = Id.ToString(),
                TotalDuration = TotalDurationMonths,
                StartDate = Purchased.ToString("yyyy-MM-dd"),
                EndDate = Expires?.ToString("yyyy-MM-dd"),
                PurchasedProducts = Products.Select(product => product.ToDTO())
            };
        }
    }
}
