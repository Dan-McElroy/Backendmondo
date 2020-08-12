﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Backendmondo.API.Models.DTOs
{
    public class SubscriptionDTO
    {
        public string Id { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public IEnumerable<ProductDTO> PurchasedProducts { get; set; }

        public int TotalDuration { get; set; }
    }
}
