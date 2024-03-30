using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly ShopWebDbContext shopWebDbContext;

        public PurchaseRepository(ShopWebDbContext shopWebDbContext)
        {
            this.shopWebDbContext = shopWebDbContext;
        }

        public async Task<PurchaseItem> AddPurchaseItem(PurchaseItem purchaseItem)
        {
            if (purchaseItem == null)
                throw new ArgumentNullException(nameof(purchaseItem));

            await shopWebDbContext.PurchaseItems.AddAsync(purchaseItem);
            await shopWebDbContext.SaveChangesAsync();
            return purchaseItem;
        }

        public async Task<IEnumerable<PurchaseItem>> GetAllPurchaseItems(Guid PurchaseId)
        {
            return await shopWebDbContext.PurchaseItems.Where(x => x.PurchaseId == PurchaseId).ToListAsync();
        }

        public async Task<IEnumerable<Purchase>> GetAllPurchases()
        {
            return await shopWebDbContext.Purchase.ToListAsync();
        }

        public async Task<Purchase> GetCurrentUserPurchaseAsync(Guid userId)
        {
            return await shopWebDbContext.Purchase.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Purchase?> GetPurchaseById(Guid id)
        {
            var purchase =  await shopWebDbContext.Purchase.FirstOrDefaultAsync(x => x.Id == id);  
            return purchase;
        }

        public async Task<Purchase> SavePurchaseAsync(Purchase purchase)
        {
            // Implement logic to save purchase to the database
            await shopWebDbContext.Purchase.AddAsync(purchase);
            await shopWebDbContext.SaveChangesAsync();
            return purchase;
        }
    }
}
