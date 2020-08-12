namespace Backendmondo.API.Models.DTOs
{
    public class ProductDTO
    {
        public string ProductId { get; set; }

        public string Name { get; set; }

        public int DurationMonths { get; set; }

        public float PriceUSD { get; set; }

        public float TaxUSD { get; set; }
    }
}
