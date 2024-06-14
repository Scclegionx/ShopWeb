using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ShopWeb.Data;
using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public class VariantAttributesRepository : IVariantAttributesRepository
    {
        private readonly ShopWebDbContext shopWebDbContext;

        public VariantAttributesRepository(ShopWebDbContext shopWebDbContext)
        {
            this.shopWebDbContext = shopWebDbContext;
        }

        public async Task AddAsync(VariantAttribute variantAttribute)
        {
            await shopWebDbContext.VariantAttributes.AddAsync(variantAttribute);
            await shopWebDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var variantAttribute = await shopWebDbContext.VariantAttributes.FindAsync(id);
            if (variantAttribute != null)
            {
                shopWebDbContext.VariantAttributes.Remove(variantAttribute);
                await shopWebDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<VariantAttribute>> GetAllVariantsAttributeByVariantAsync(Guid productVariantId)
        {
            return await shopWebDbContext.VariantAttributes
                .Where(v => v.ProductVariantId == productVariantId)
                .ToListAsync();
        }

        public async Task<List<VariantAttribute>> GetByProductVariantIdAsync(Guid productVariantId)
        {
            return await shopWebDbContext.VariantAttributes
                .Where(va => va.ProductVariantId == productVariantId)
                .ToListAsync();
        }

        public async Task<Guid> GetProductVariantIdByKeyAValue(string KnV)
        {
            var values = KnV.Split(' ');

            if (values.Length == 0)
            {
                throw new ArgumentException("KnV must contain at least one value.");
            }

            // Thực hiện truy vấn
            var productVariantId = await shopWebDbContext.VariantAttributes
                .Where(p => values.Contains(p.Value))
                .GroupBy(p => p.ProductVariantId)
                .Where(g => g.Select(p => p.Value).Distinct().Count() == values.Length)
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            return productVariantId;
        }

        public async Task UpdateAsync(VariantAttribute variantAttribute)
        {
            shopWebDbContext.Entry(variantAttribute).State = EntityState.Modified;
            await shopWebDbContext.SaveChangesAsync();
        }

        public async Task UpdateProductVariantQuantity(Guid productVariantId, int quantity)
        {
            var productVariant = await shopWebDbContext.ProductVariants.FindAsync(productVariantId);
            if (productVariant != null)
            {
                productVariant.Quantity -= quantity;
                await shopWebDbContext.SaveChangesAsync();
            }
        }
    }
}
