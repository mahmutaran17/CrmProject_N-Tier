using CrmProject.Business.Abstract;
using CrmProject.DataAccess.Abstract;
using CrmProject.Entity.Entities;

namespace CrmProject.Business.Concrete
{
    public class TaskLogManager : GenericManager<TaskLog>, ITaskLogService
    {
        private readonly ITaskLogRepository _taskLogRepository;

        public TaskLogManager(ITaskLogRepository taskLogRepository) : base(taskLogRepository)
        {
            _taskLogRepository = taskLogRepository;
        }
    }
}