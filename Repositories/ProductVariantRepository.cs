using ShopWeb.Data;
using ShopWeb.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Ajax.Utilities;

namespace ShopWeb.Repositories
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly ShopWebDbContext shopWebDbContext;

        public ProductVariantRepository(ShopWebDbContext shopWebDbContext)
        {
            this.shopWebDbContext = shopWebDbContext;
        }
        public async Task AddAsync(ProductVariant productVariant)
        {
            await shopWebDbContext.ProductVariants.AddAsync(productVariant);
            await shopWebDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var productVariant = await shopWebDbContext.ProductVariants.FindAsync(id);
            if (productVariant != null)
            {
                shopWebDbContext.ProductVariants.Remove(productVariant);
                await shopWebDbContext.SaveChangesAsync();
            }
        }

        public async Task<ProductVariant> GetAsync(Guid id)
        {
            return await shopWebDbContext.ProductVariants.FindAsync(id);
        }

        public async Task<List<ProductVariant>> GetVariantsByProductIdAsync(Guid productId)
        {
            return await shopWebDbContext.ProductVariants
                .Where(v => v.ProductId == productId)
                .Include(v => v.Attributes)
                .ToListAsync();
        }

        public async Task UpdateAsync(ProductVariant productVariant)
        {
            shopWebDbContext.Entry(productVariant).State = EntityState.Modified;
            await shopWebDbContext.SaveChangesAsync();
        }
    }
}
