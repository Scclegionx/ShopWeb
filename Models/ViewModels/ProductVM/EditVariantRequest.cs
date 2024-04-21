namespace ShopWeb.Models.ViewModels.ProductVM
{
    public class EditVariantRequest
    {
        public Guid VariantId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public List<VariantAttributeRequestForEdit> VariantAttributes { get; set; }
    }
}
