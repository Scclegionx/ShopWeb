using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopWeb.Models.Domain;
using ShopWeb.Repositories;

namespace ShopWeb.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationRepository notificationRepository;
        private readonly UserManager<ApplicationUser> userManager;
        public NotificationController(INotificationRepository notificationRepository, UserManager<ApplicationUser> userManager)
        {
            this.notificationRepository = notificationRepository;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var notifications = await notificationRepository.GetNotificationsForUserAsync(Guid.Parse(user.Id));
            return Json(notifications.Select(n => new { n.Title, n.Message, n.CreatedAt }));
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(Guid notificationId)
        {
            await notificationRepository.MarkAsReadAsync(notificationId);
            return Ok();
        }
    }
}
