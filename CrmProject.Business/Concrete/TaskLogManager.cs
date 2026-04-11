using CrmProject.Business.Abstract;
using CrmProject.DataAccess.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.Business.Concrete
{
    public class TaskLogManager : GenericManager<TaskLog>, ITaskLogService
    {
        private readonly ITaskLogRepository _taskLogRepository;
        private readonly IAppTaskRepository _appTaskRepository;

        public TaskLogManager(ITaskLogRepository taskLogRepository, IAppTaskRepository appTaskRepository) : base(taskLogRepository)
        {
            _taskLogRepository = taskLogRepository;
            _appTaskRepository = appTaskRepository;
        }

        public async Task<List<TaskLog>> GetAllLogsWithTaskAsync()
        {
            return await _taskLogRepository.GetListWithIncludesAsync(null, x => x.TaskItem);
        }

        public async Task<SelectList> GetTasksForDropdownAsync(int? selectedTaskId = null)
        {
            var tasks = await _appTaskRepository.GetAllAsync();
            return new SelectList(tasks, "Id", "Title", selectedTaskId);
        }

        public async Task<(bool Success, string Message, int TaskItemId)> CreateLogAsync(TaskLog taskLog)
        {
            taskLog.CreatedAt = DateTime.Now;

            await _taskLogRepository.AddAsync(taskLog);
            await _taskLogRepository.SaveAsync();
            return (true, "Görev geçmişine yeni bir not başarıyla eklendi.", taskLog.TaskItemId);
        }
    }
}