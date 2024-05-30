using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public interface ICateRepository
    {
        Task<IEnumerable<Category>> GetAllAsync(int? page, int? pageSize);
        Task<Category> GetAsync(Guid id);
        Task<Category> AddAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task<Category> DeleteAsync(Guid id);
        Task<int> GetTotalCategoryCount();
    }
}
