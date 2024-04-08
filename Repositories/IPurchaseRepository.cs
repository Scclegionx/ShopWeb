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
        Task<Purchase?> DeletePurchaseAsync(Guid id);
        Task<IEnumerable<Purchase>> GetOwnPurchases(Guid userId);
        Task UpdatePurchaseAsync(Purchase purchase);
        Task<IEnumerable<Purchase>> GetPurchaseByShipperId(Guid shipperId);
        Task<IEnumerable<Purchase>> GetOwnPurchaseForTracking(Guid userId);
        Task<IEnumerable<Purchase>> GetOwnPurchaseForHistory(Guid userId);
    }
}
