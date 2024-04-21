using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public interface IVariantAttributesRepository
    {
        Task AddAsync(VariantAttribute variantAttribute);
        Task<List<VariantAttribute>> GetAllVariantsAttributeByVariantAsync(Guid productVariantId);
        Task<List<VariantAttribute>> GetByProductVariantIdAsync(Guid productVariantId);
        Task UpdateAsync(VariantAttribute variantAttribute);
        Task DeleteAsync(Guid id);
    }
}
