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

        // Tüm Bildirimleri Gösteren Tam Sayfa
        public async Task<IActionResult> Index()
        {
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Index", "Login");

            int currentUserId = int.Parse(userIdStr);

            // Kullanıcının tüm bildirimlerini tarihe göre azalan şekilde (en yeni en üstte) getir
            var notifications = await _notificationService.GetWhereAsync(n => n.UserId == currentUserId);
            var sortedNotifications = notifications.OrderByDescending(n => n.CreatedAt).ToList();

            return View(sortedNotifications);
        }

        // Bildirime Tıklandığında Okundu İşaretle ve Görevlere Yönlendir
        public async Task<IActionResult> ReadAndRedirect(int id)
        {
            var notification = await _notificationService.GetByIdAsync(id);
            if (notification != null)
            {
                notification.IsRead = true;
                _notificationService.Update(notification);
                await _notificationService.SaveAsync();
            }

            // Şimdilik hepsi görev bildirimi olduğu için Görevler sayfasına yönlendiriyoruz
            // İleride farklı bildirim tipleri (Gelir vs.) eklersen buraya bir IF mantığı kurabilirsin
            return RedirectToAction("Index", "AppTask");
        }

        // Çan Menüsündeki "Tümünü Okundu İşaretle" Butonu İçin
        public async Task<IActionResult> MarkAllRead()
        {
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Index", "Login");

            int currentUserId = int.Parse(userIdStr);
            var unreadNotifications = await _notificationService.GetWhereAsync(n => n.UserId == currentUserId && !n.IsRead);

            foreach (var notif in unreadNotifications)
            {
                notif.IsRead = true;
                _notificationService.Update(notif);
            }

            await _notificationService.SaveAsync();

            return RedirectToAction("Index", "Home"); // Ana sayfaya geri dön
        }
    }
}