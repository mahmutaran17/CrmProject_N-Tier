using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.Business.Abstract
{
    public interface IProjectService : IGenericService<Project>
    {
        Task<List<Project>> GetActiveProjectsAsync();
        Task<SelectList> GetActiveCustomersForDropdownAsync(int? selectedCustomerId = null);
        Task<(bool Success, string Message)> AddProjectAsync(Project project);
        Task<(bool Success, string Message)> UpdateProjectAsync(Project project);
        Task<(bool Success, string Message)> SoftDeleteProjectAsync(int id);
        Task<(Project Project, List<AppTask> Tasks, List<Income> Incomes, List<Expense> Expenses, decimal Balance)?> GetProjectDetailsAsync(int id);
    }
}
