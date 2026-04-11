using CrmProject.Entity.Entities;

namespace CrmProject.Business.Abstract
{
    public interface INotificationService : IGenericService<Notification>
    {
        Task<List<Notification>> GetUserNotificationsSortedAsync(int userId);
        Task MarkAsReadAsync(int id);
        Task MarkAllAsReadAsync(int userId);
    }
}
