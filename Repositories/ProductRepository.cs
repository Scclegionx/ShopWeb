using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopWebDbContext shopWebDbContext;
        public ProductRepository(ShopWebDbContext shopWebDbContext)
        {
            this.shopWebDbContext = shopWebDbContext;
        }
        public async Task<Product> AddAsync(Product product)
        {
            await shopWebDbContext.Products.AddAsync(product);
            await shopWebDbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> DeleteAsync(Guid id)
        {
            var existingProd = await shopWebDbContext.Products.FindAsync(id);
            if (existingProd != null)
            {
                shopWebDbContext.Products.Remove(existingProd);
                await shopWebDbContext.SaveChangesAsync();
                return existingProd;
            }
            return null;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await shopWebDbContext.Products.Include(x => x.Categories).ToListAsync();
        }

        public async Task<Product?> GetAsync(Guid id)
        {
            return await shopWebDbContext.Products.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);    
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existingProd = await shopWebDbContext.Products.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == product.Id);
            if (existingProd != null)
            {
                existingProd.Id = product.Id;
                existingProd.Name = product.Name;
                existingProd.Description = product.Description;
                existingProd.FeaturedImageUrl = product.FeaturedImageUrl;
                existingProd.Price = product.Price;
                existingProd.Quantity = product.Quantity;
                existingProd.Categories = product.Categories;

                await shopWebDbContext.SaveChangesAsync();
                return existingProd;
            }
            return null;
        }
    }
}
