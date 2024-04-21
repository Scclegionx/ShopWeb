using ShopWeb.Data;
using ShopWeb.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ShopWeb.Repositories
{
    public class ProductRatingRepository : IProductRatingRepository
    {
        private readonly ShopWebDbContext shopWebDbContext;

        public ProductRatingRepository(ShopWebDbContext shopWebDbContext)
        {
            this.shopWebDbContext = shopWebDbContext;
        }
        public async Task AddAsync(ProductRating rating)
        {
            shopWebDbContext.ProductRatings.Add(rating);
            await shopWebDbContext.SaveChangesAsync();
        }

        public async Task<double> GetAverageRating(Guid productId)
        {
            var averageRating = shopWebDbContext.ProductRatings.Where(r => r.ProductId == productId).Average(r => r.Rating);
            return averageRating;
        }

        public async Task<ProductRating> GetRatingByUserAndProduct(Guid userId, Guid productId)
        {
            return await shopWebDbContext.ProductRatings.FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);
        }

        public async Task UpdateAsync(ProductRating rating)
        {
            shopWebDbContext.ProductRatings.Update(rating);
            await shopWebDbContext.SaveChangesAsync();
        }
    }
}
