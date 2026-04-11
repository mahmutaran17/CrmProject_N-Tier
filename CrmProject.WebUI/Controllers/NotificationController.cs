using CrmProject.Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmProject.WebUI.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Index", "Login");

            int currentUserId = int.Parse(userIdStr);
            var notifications = await _notificationService.GetUserNotificationsSortedAsync(currentUserId);
            return View(notifications);
        }

        public async Task<IActionResult> ReadAndRedirect(int id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return RedirectToAction("Index", "AppTask");
        }

        public async Task<IActionResult> MarkAllRead()
        {
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Index", "Login");

            int currentUserId = int.Parse(userIdStr);
            await _notificationService.MarkAllAsReadAsync(currentUserId);
            return RedirectToAction("Index", "Home");
        }
    }
}