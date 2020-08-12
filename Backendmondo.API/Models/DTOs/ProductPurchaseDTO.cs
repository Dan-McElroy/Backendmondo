namespace Backendmondo.API.Models.DTOs
{
    public class ProductPurchaseDTO : ProductDTO
    {
        public string DateOfPurchase { get;set; }
        
        public float PriceWhenPurchased { get; set; }
        
        public float TaxWhenPurchased { get; set; }
    }
}
