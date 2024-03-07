
using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public class ProductLikeRepository : IProductLikeRepository
    {
        private readonly ShopWebDbContext shopWebDbContext;

        public ProductLikeRepository(ShopWebDbContext shopWebDbContext)
        {
            this.shopWebDbContext = shopWebDbContext;
        }

        public async Task<ProductLike> AddLiketoProduct(ProductLike productLike)
        {
            await shopWebDbContext.ProductLike.AddAsync(productLike);
            await shopWebDbContext.SaveChangesAsync();
            return productLike;
        }

        public async Task<IEnumerable<ProductLike>> GetAllLikes(Guid productId)
        {
            return await shopWebDbContext.ProductLike.Where(x =>  x.ProductId == productId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid productId)
        {
            return await shopWebDbContext.ProductLike.CountAsync(x => x.ProductId == productId);
        }
    }
}
