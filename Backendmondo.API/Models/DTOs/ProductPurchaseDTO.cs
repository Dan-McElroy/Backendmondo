namespace Backendmondo.API.Models.DTOs
{
    public class ProductPurchaseDTO : ProductDTO
    {
        public string DateOfPurchase { get;set; }
        
        public float PriceUSDWhenPurchased { get; set; }
        
        public float TaxUSDWhenPurchased { get; set; }
    }
}
