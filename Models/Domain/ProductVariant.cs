namespace ShopWeb.Models.Domain
{
    public class ProductVariant
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; } // Foreign key to relate to the Product
        public string Size { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string FeaturedImageUrl { get; set; }

        // Navigation property to relate to the Product
        public Product Product { get; set; }
    }
}
