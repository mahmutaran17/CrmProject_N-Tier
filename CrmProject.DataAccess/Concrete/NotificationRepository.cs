using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmProject.DataAccess.Abstract;
using CrmProject.DataAccess.Context;
using CrmProject.Entity.Entities;

namespace CrmProject.DataAccess.Concrete
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(CrmDbContext context) : base(context)
        {
        }
    }
}
