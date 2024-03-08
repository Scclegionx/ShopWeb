using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    
    public class ProductCommentRepository : IProductCommentRepository
    {
        private readonly ShopWebDbContext shopWebDbContext;

        public ProductCommentRepository(ShopWebDbContext shopWebDbContext)
        {
            this.shopWebDbContext = shopWebDbContext;
        }
        public async Task<ProductComment> AddAsync(ProductComment productComment)
        {
            await shopWebDbContext.ProductComment.AddAsync(productComment);
            await shopWebDbContext.SaveChangesAsync();
            return productComment;
        }

        public async Task<IEnumerable<ProductComment>> GetAllAsync(Guid productId)
        {
           return await shopWebDbContext.ProductComment.Where(x =>  x.ProductId == productId).ToListAsync();
        }
    }
}
