using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(int? page, int? pageSize);
        Task<Product?> GetAsync(Guid id);
        Task<Product> AddAsync(Product product);
        Task<Product?> UpdateAsync(Product product);
        Task<Product?> DeleteAsync(Guid id);
        Task<IEnumerable<Product?>> FindByNameAsync(string productName);
        Task<int> GetTotalProductsCount();
        Task<List<Product>> GetProductsByCategoryAsync(string category);
        Task<IEnumerable<Product>> FindByNameAndCategoryAsync(string productName, string category);
        Task UpdateProductQuantity(Guid productId, int quantity);
        Task CheckProductAvailability(Guid productId);
        Task<Product?> UpdateRatingAsync(Guid id);
        Task<Product?> UpdateCommentCountAsync(Guid id, int totalComments);
    }
}
