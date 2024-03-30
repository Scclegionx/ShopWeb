using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public interface IPurchaseRepository
    {
        Task<Purchase> SavePurchaseAsync(Purchase purchase);
        Task<IEnumerable<Purchase>> GetAllPurchases();
        Task<Purchase?> GetPurchaseById(Guid id);
        Task<PurchaseItem> AddPurchaseItem(PurchaseItem purchaseItem);
        Task<Purchase> GetCurrentUserPurchaseAsync(Guid userId);
        Task<IEnumerable<PurchaseItem>> GetAllPurchaseItems(Guid PurchaseId);
    }
}
