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

        public Product Product { get; set; }

        public DateTime Purchased { get; set; }

        public ICollection<SubscriptionPause> Pauses { get; set; }

        public bool IsPaused => Pauses.Any(pause => pause.IsOngoing);

        public DateTime? Expires
        {
            get
            {
                if (IsPaused)
                {
                    return null;
                }
                return Purchased.AddMonths(Product.DurationMonths) + TotalPauseDuration;
            }
        }

        public Subscription()
        {
            Pauses = new List<SubscriptionPause>();
        }

        private TimeSpan TotalPauseDuration =>
            TimeSpan.FromMilliseconds(Pauses
                .Where(pause => !pause.IsOngoing)
                .Sum(pause => pause.Duration.TotalMilliseconds));


        public SubscriptionDTO ToDTO()
        {
            return new SubscriptionDTO
            {
                Id = Id.ToString(),
                Duration = Product.DurationMonths,
                StartDate = Purchased.ToString("yyyy-MM-dd"),
                EndDate = Expires?.ToString("yyyy-MM-dd")
            };
        }
    }
}
