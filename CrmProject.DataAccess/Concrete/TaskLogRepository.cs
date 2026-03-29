using CrmProject.DataAccess.Abstract;
using CrmProject.DataAccess.Context;
using CrmProject.Entity.Entities;

namespace CrmProject.DataAccess.Concrete
{
    public class TaskLogRepository : GenericRepository<TaskLog>, ITaskLogRepository
    {
        public TaskLogRepository(CrmDbContext context) : base(context)
        {
        }
    }
}