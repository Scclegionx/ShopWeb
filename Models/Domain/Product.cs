namespace ShopWeb.Models.Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FeaturedImageUrl { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<ProductLike> ProductLike { get; set; }
        public ICollection<ProductComment> ProductComment { get; set; }
        public int PurchaseCount { get; set; }
        public string State { get; set; }
        public int Rating { get; set; }
        public int CommentsCount { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public bool IsSale { get; set; } // Indicates if the product is on sale
        public decimal? SalePrice { get; set; } // The sale price of the product
        public DateTime? SaleEndDate { get; set; }
    }
}
