using ShopWeb.Models.Domain;

namespace ShopWeb.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetNotificationsForUserAsync(Guid userId);
        Task MarkAsReadAsync(Guid notificationId);
        Task AddNotificationAsync(Notification notification);
        Task<int> GetNotificationCountByUserIdAsync(Guid userId);
    }
}
