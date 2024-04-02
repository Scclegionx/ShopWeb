using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models.Domain;
using PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Ajax.Utilities;

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

        public async Task<IEnumerable<Product?>> FindByNameAsync(string productName)
        {
            var existProducts = await shopWebDbContext.Products.Where(x => x.Name.Contains(productName)).ToListAsync();
            return existProducts;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(int? page, int? pageSize)
        {
            // Calculate the number of items to skip
            if (page.HasValue && pageSize.HasValue)
            {
                int skip = (page.Value - 1) * pageSize.Value;
                return await shopWebDbContext.Products.Include(x => x.Categories).Skip(skip).Take(pageSize.Value).ToListAsync();
            }
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

        public async Task<int> GetTotalProductsCount()
        {
            return await shopWebDbContext.Products.CountAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(string category)
        {
            var products = await shopWebDbContext.Products
                .Where(p => p.Categories.Any(c => c.Name == category))
                .ToListAsync();

            return products;
        }

        public async Task<IEnumerable<Product>> FindByNameAndCategoryAsync(string productName, string category)
        {
            return await shopWebDbContext.Products
                .Include(p => p.Categories)
                .Where(p => p.Name.Contains(productName) && p.Categories.Any(c => c.Name == category))
                .ToListAsync();
        }
    }
}
