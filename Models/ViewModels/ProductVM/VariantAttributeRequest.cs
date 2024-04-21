namespace ShopWeb.Models.ViewModels.ProductVM
{
    public class VariantAttributeRequest
    {
        public List<string> Name { get; set; }
        public List<string> Value { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid ProductVariantId { get; set; }
    }
}
