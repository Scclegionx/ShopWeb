using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public interface ICateRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetAsync(Guid id);
        Task<Category> AddAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task<Category> DeleteAsync(Guid id);
    }
}
