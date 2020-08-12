using System.Collections.Generic;

namespace Backendmondo.API.Models.DTOs
{
    public class SubscriptionDTO
    {
        public string Id { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public IEnumerable<ProductPurchaseDTO> PurchasedProducts { get; set; }

        public int TotalDurationMonths { get; set; }

        public float TotalPurchaseCostUSD { get; set; }

        public float TotalTaxCostUSD { get; set; }

        public string DatePaused { get; set; }
    }
}
