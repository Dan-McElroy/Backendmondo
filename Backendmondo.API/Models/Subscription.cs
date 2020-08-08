using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backendmondo.API.Models
{
    public class Subscription
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public User User { get; set; }

        public Product Product { get; set; }

        public DateTime Purchased { get; set; }

        public ICollection<SubscriptionPause> Pauses { get; set; }
    }
}
