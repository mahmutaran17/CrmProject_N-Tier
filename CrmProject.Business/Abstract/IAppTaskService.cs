using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.Business.Abstract
{
    public interface IAppTaskService
    {
        Task<IEnumerable<AppTask>> GetTasksByUserRoleAsync(int currentUserId, bool isAdmin);
        Task<AppTask?> GetTaskDetailsByIdAsync(int id);
        Task<(bool Success, string Message)> CreateTaskWithRelationsAsync(AppTask task, List<int> selectedUserIds, int assignedByUserId);
        Task<(bool Success, string Message)> UpdateTaskWithRelationsAsync(AppTask task, List<int> selectedUserIds);
        Task<(bool Success, string Message)> UpdateTaskStatusAsync(int taskId, AppTaskStatus status);
        Task<SelectList> GetActiveProjectsForDropdownAsync(int? selectedProjectId = null);
        Task<SelectList> GetActiveUsersForDropdownAsync();
    }
}