using ShopWeb.Data;
using ShopWeb.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Ajax.Utilities;

namespace ShopWeb.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ShopWebDbContext shopWebDbContext;

        public NotificationRepository(ShopWebDbContext shopWebDbContext) 
        {
            this.shopWebDbContext = shopWebDbContext;
        }
        public async Task AddNotificationAsync(Notification notification)
        {
            await shopWebDbContext.Notifications.AddAsync(notification);
            await shopWebDbContext.SaveChangesAsync();
        }

        public async Task<int> GetNotificationCountByUserIdAsync(Guid userId)
        {
            return await shopWebDbContext.Notifications
            .Where(n => n.UserId == userId)
            .CountAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsForUserAsync(Guid userId)
        {
            return await shopWebDbContext.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
        }

        public async Task MarkAsReadAsync(Guid notificationId)
        {
            var notification = await shopWebDbContext.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await shopWebDbContext.SaveChangesAsync();
            }
        }
    }
}
