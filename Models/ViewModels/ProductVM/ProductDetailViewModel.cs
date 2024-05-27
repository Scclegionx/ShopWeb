using ShopWeb.Models.Domain;

namespace ShopWeb.Models.ViewModels.ProductVM
{
    public class ProductDetailViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FeaturedImageUrl { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string CommentDescription { get; set; }
        public int CommentsCount { get; set; }
        public List<List<VariantAttribute>> Variants { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<ProductLike> ProductLike { get; set; }
        public IEnumerable<ProductCommentViewModel> Comments { get; set; }
        public int Rating { get; set; }
        public List<string> AdditionalImageUrls { get; set; }
    }
}
