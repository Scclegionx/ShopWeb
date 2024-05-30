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
        Task AddImageAsync(ProductImage productImage);
        Task<List<ProductImage>> GetAdditionalImagesAsync(Guid productId);
        Task<List<ProductImage>> GetAdditionalImagesByProductIdAsync(Guid productId);
        Task DeleteImageAsync(ProductImage productImage);
        Task<List<Product>> GetProductsByCategoryForHomeAsync(string category);
        Task<IEnumerable<Product>> GetSaleProductsAsync();
        Task SetSaleAsync(Guid productId, decimal salePrice, DateTime saleEndDate);
        Task RemoveSaleAsync(Guid productId);
        Task<IEnumerable<Product>> GetAllBySortAsync(int page, int pageSize, string sortBy, bool sortDescending, string category);
        Task<int> GetTotalProductsCountAfterSort(string category);
    }
}
