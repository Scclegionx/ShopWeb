using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public interface IProductRatingRepository
    {
        Task<ProductRating> GetRatingByUserAndProduct(Guid userId, Guid productId);
        Task AddAsync(ProductRating rating);
        Task UpdateAsync(ProductRating rating);
        Task<double> GetAverageRating(Guid productId);
    }
}
