using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models.Domain;
using ShopWeb.Models.ViewModels;

namespace ShopWeb.Repositories
{
    public class CateRepository : ICateRepository
    {
        private readonly ShopWebDbContext shopWebDbContext;
        public CateRepository(ShopWebDbContext shopWebDbContext)
        {
            this.shopWebDbContext = shopWebDbContext;
        }
        public async Task<Category> AddAsync(Category category)
        {
            await shopWebDbContext.Categories.AddAsync(category);
            await shopWebDbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var exist = await shopWebDbContext.Categories.FindAsync(id);
            if (exist != null)
            {
                shopWebDbContext.Categories.Remove(exist);
                await shopWebDbContext.SaveChangesAsync();
                return exist;
            }
            return null;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await shopWebDbContext.Categories.ToListAsync();
        }

        public Task<Category?> GetAsync(Guid id)
        {
            return shopWebDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCate = await shopWebDbContext.Categories.FindAsync(category.Id);
            if (existingCate != null)
            {
                existingCate.Name = category.Name;
                existingCate.Description = category.Description;

                await shopWebDbContext.SaveChangesAsync();

                return existingCate;
            }
            return null;
        }
    }
}
