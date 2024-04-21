using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models.Domain;
using ShopWeb.Models.ViewModels.UserVM;

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

        public async Task<Purchase?> DeletePurchaseAsync(Guid id)
        {
            var purchase = await shopWebDbContext.Purchase.FindAsync(id);
            if (purchase != null)
            {
                shopWebDbContext.Purchase.Remove(purchase);
                await shopWebDbContext.SaveChangesAsync();
            }
            return null;
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

        public async Task<IEnumerable<Purchase>> GetOwnPurchaseForHistory(Guid userId)
        {
            return await shopWebDbContext.Purchase.Where(x => x.UserId == userId && x.State == "Done").ToListAsync();
        }

        public async Task<IEnumerable<Purchase>> GetOwnPurchaseForTracking(Guid userId)
        {
            return await shopWebDbContext.Purchase.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Purchase>> GetOwnPurchases(Guid userId)
        {
            return await shopWebDbContext.Purchase.Where(x => x.UserId == userId && x.State != "Done").ToListAsync();
        }

        public async Task<Purchase?> GetPurchaseById(Guid id)
        {
            var purchase =  await shopWebDbContext.Purchase.FirstOrDefaultAsync(x => x.Id == id);  
            return purchase;
        }

        public async Task<IEnumerable<Purchase>> GetPurchaseByShipperId(Guid shipperId)
        {
            return await shopWebDbContext.Purchase.Where(x => x.ShipperID == shipperId && x.State != "Done").ToListAsync();
        }

        public async Task<Purchase> SavePurchaseAsync(Purchase purchase)
        {
            // Implement logic to save purchase to the database
            await shopWebDbContext.Purchase.AddAsync(purchase);
            await shopWebDbContext.SaveChangesAsync();
            return purchase;
        }

        public async Task UpdatePurchaseAsync(Purchase purchase)
        {
            shopWebDbContext.Entry(purchase).State = EntityState.Modified;
            await shopWebDbContext.SaveChangesAsync();
        }
        public async Task UpdatePurchaseCount(Guid productId, int quantity)
        {
            var product = await shopWebDbContext.Products.FindAsync(productId);
            if (product != null)
            {
                product.PurchaseCount += quantity;
                await shopWebDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetBestSellingProducts()
        {
            return await shopWebDbContext.Products.OrderByDescending(p => p.PurchaseCount)
                                            .Take(5)
                                            .ToListAsync();
        }


    }
}
