namespace ShopWeb.Models.Domain
{
    public class ProductVariant
    {
        public Guid Id { get; set; }
        public List<VariantAttribute> Attributes { get; set; } // Dynamic attributes
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
    public class VariantAttribute
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public Guid ProductVariantId { get; set; }
    }
}
