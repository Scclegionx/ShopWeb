using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public interface IProductVariantRepository
    {
        Task AddAsync(ProductVariant productVariant);
        Task<List<ProductVariant>> GetVariantsByProductIdAsync(Guid productId);

        Task<ProductVariant> GetAsync(Guid id);
        Task UpdateAsync(ProductVariant productVariant);
        Task DeleteAsync(Guid id);
    }
}
