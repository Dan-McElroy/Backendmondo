using System;

namespace Backendmondo.API.Models.DTOs
{
    public class SubscriptionDTO
    {
        public string Id { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public float Price { get; set; }

        public float Tax { get; set; }

        public int Duration { get; set; }
    }
}
