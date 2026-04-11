using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.Business.Abstract
{
    public interface ITaskLogService : IGenericService<TaskLog>
    {
        Task<List<TaskLog>> GetAllLogsWithTaskAsync();
        Task<SelectList> GetTasksForDropdownAsync(int? selectedTaskId = null);
        Task<(bool Success, string Message, int TaskItemId)> CreateLogAsync(TaskLog taskLog);
    }
}