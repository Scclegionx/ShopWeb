namespace ShopWeb.Models.Domain
{
    public class PurchaseItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid PurchaseId { get; set; }
        public Guid ProductVariantId { get; set; }
        public Product Product { get; set; }
    }
}
