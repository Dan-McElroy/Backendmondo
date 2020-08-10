using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
