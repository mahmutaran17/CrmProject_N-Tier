using CrmProject.Business.Abstract;
using CrmProject.DataAccess.Abstract;
using CrmProject.Entity.Entities;

namespace CrmProject.Business.Concrete
{
    public class NotificationManager : GenericManager<Notification>, INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationManager(INotificationRepository notificationRepository) : base(notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<List<Notification>> GetUserNotificationsSortedAsync(int userId)
        {
            var notifications = await _notificationRepository.GetWhereAsync(n => n.UserId == userId);
            return notifications.OrderByDescending(n => n.CreatedAt).ToList();
        }

        public async Task MarkAsReadAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification != null)
            {
                notification.IsRead = true;
                _notificationRepository.Update(notification);
                await _notificationRepository.SaveAsync();
            }
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var unread = await _notificationRepository.GetWhereAsync(n => n.UserId == userId && !n.IsRead);
            foreach (var notif in unread)
            {
                notif.IsRead = true;
                _notificationRepository.Update(notif);
            }
            await _notificationRepository.SaveAsync();
        }
    }
}
