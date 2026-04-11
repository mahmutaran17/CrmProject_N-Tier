using CrmProject.Business.Abstract;
using CrmProject.DataAccess.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.Business.Concrete
{
    public class AppTaskManager : IAppTaskService
    {
        private readonly IAppTaskRepository _appTaskRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly INotificationService _notificationService;
        private readonly IGenericRepository<Project> _projectRepository;

        public AppTaskManager(
            IAppTaskRepository appTaskRepository,
            IGenericRepository<User> userRepository,
            INotificationService notificationService,
            IGenericRepository<Project> projectRepository)
        {
            _appTaskRepository = appTaskRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<AppTask>> GetTasksByUserRoleAsync(int currentUserId, bool isAdmin)
        {
            if (isAdmin)
                return await _appTaskRepository.GetListWithIncludesAsync(null, x => x.Project, x => x.AssignedByUser);

            return await _appTaskRepository.GetListWithIncludesAsync(
                x => x.AssignedUsers.Any(u => u.Id == currentUserId),
                x => x.Project,
                x => x.AssignedByUser);
        }

        public async Task<AppTask?> GetTaskDetailsByIdAsync(int id)
        {
            var taskList = await _appTaskRepository.GetListWithIncludesAsync(
                x => x.Id == id, y => y.Project, y => y.AssignedByUser, y => y.AssignedUsers, y => y.TaskLogs);
            return taskList.FirstOrDefault();
        }

        public async Task<(bool Success, string Message)> CreateTaskWithRelationsAsync(AppTask task, List<int> selectedUserIds, int assignedByUserId)
        {
            task.AssignedByUserId = assignedByUserId;

            if (selectedUserIds != null && selectedUserIds.Any())
            {
                foreach (var userId in selectedUserIds)
                {
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user != null) task.AssignedUsers.Add(user);
                }
            }

            await _appTaskRepository.AddAsync(task);
            await _appTaskRepository.SaveAsync();

            if (selectedUserIds != null && selectedUserIds.Any())
            {
                foreach (var userId in selectedUserIds)
                {
                    var notification = new Notification
                    {
                        UserId = userId,
                        Message = $"'{task.Title}' başlıklı yeni bir görev size atandı.",
                        IsRead = false,
                        CreatedAt = DateTime.Now
                    };
                    await _notificationService.AddAsync(notification);
                }
                await _notificationService.SaveAsync();
            }

            return (true, $"'{task.Title}' başlıklı görev başarıyla oluşturuldu ve bildirimler gönderildi.");
        }

        public async Task<(bool Success, string Message)> UpdateTaskWithRelationsAsync(AppTask task, List<int> selectedUserIds)
        {
            var existingTasks = await _appTaskRepository.GetListWithIncludesAsync(x => x.Id == task.Id, y => y.AssignedUsers);
            var existingTask = existingTasks.FirstOrDefault();

            if (existingTask == null)
                return (false, "Güncellenecek görev bulunamadı.");

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.ProjectId = task.ProjectId;
            existingTask.DueDate = task.DueDate;
            existingTask.Priority = task.Priority;
            existingTask.Status = task.Status;

            existingTask.AssignedUsers.Clear();

            if (selectedUserIds != null && selectedUserIds.Any())
            {
                foreach (var userId in selectedUserIds)
                {
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user != null) existingTask.AssignedUsers.Add(user);
                }
            }

            _appTaskRepository.Update(existingTask);
            await _appTaskRepository.SaveAsync();
            return (true, $"'{task.Title}' başlıklı görev başarıyla güncellendi.");
        }

        public async Task<(bool Success, string Message)> UpdateTaskStatusAsync(int taskId, AppTaskStatus status)
        {
            var task = await _appTaskRepository.GetByIdAsync(taskId);
            if (task == null) return (false, "Görev bulunamadı.");

            task.Status = status;
            _appTaskRepository.Update(task);
            await _appTaskRepository.SaveAsync();
            return (true, $"'{task.Title}' başlıklı görev durumu başarıyla güncellendi.");
        }

        public async Task<SelectList> GetActiveProjectsForDropdownAsync(int? selectedProjectId = null)
        {
            var projects = await _projectRepository.GetWhereAsync(x => x.Status == ProjectStatus.Aktif);
            return new SelectList(projects, "Id", "ProjectName", selectedProjectId);
        }

        public async Task<SelectList> GetActiveUsersForDropdownAsync()
        {
            var users = await _userRepository.GetWhereAsync(x => x.IsActive);
            return new SelectList(users, "Id", "FirstName");
        }
    }
}