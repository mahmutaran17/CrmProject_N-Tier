using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
