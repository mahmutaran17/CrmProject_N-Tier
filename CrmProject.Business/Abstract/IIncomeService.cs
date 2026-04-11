using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.Business.Abstract
{
    public interface IIncomeService : IGenericService<Income>
    {
        Task<List<Income>> GetAllIncomesWithProjectAsync();
        Task<SelectList> GetActiveProjectsForDropdownAsync(int? selectedProjectId = null);
        Task<(bool Success, string Message)> CreateIncomeAsync(Income income);
        Task<(bool Success, string Message)> UpdateIncomeAsync(Income income);
        Task<(bool Success, string Message)> DeleteIncomeAsync(int id);
    }
}