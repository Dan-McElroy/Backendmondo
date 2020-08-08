using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backendmondo.API.Models.DTOs
{
    public class SubscriptionDTO
    {
        public string Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public float Price { get; set; }

        public float Tax { get; set; }

        public int Duration { get; set; }
    }
}
