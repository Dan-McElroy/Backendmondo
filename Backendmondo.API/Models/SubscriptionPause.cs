using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backendmondo.API.Models
{
    public class SubscriptionPause
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Subscription Subscription { get; set; }

        public DateTime Started { get; set; }

        public DateTime? Ended { get; set; }

        public bool IsOngoing => Ended == null;

        public TimeSpan Duration 
            => (Ended ?? Started - TimeSpan.FromSeconds(1)) - Started;
    }
}
