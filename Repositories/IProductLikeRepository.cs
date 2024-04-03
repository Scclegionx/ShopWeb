using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public interface IProductLikeRepository
    {
        Task<int> GetTotalLikes(Guid productId);
        Task<ProductLike> AddLiketoProduct(ProductLike productLike);
        Task<IEnumerable<ProductLike>> GetAllLikes(Guid productId);
        Task RemoveLikeFromProduct(Guid userId, Guid productId);
        Task<IEnumerable<Guid>> GetLikedProductIds(Guid userId);
    }
}
