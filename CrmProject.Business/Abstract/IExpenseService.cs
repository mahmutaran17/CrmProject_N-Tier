using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.Business.Abstract
{
    public interface IExpenseService : IGenericService<Expense>
    {
        Task<List<Expense>> GetAllExpensesWithProjectAsync();
        Task<SelectList> GetActiveProjectsForDropdownAsync(int? selectedProjectId = null);
        Task<(bool Success, string Message)> CreateExpenseAsync(Expense expense);
        Task<(bool Success, string Message)> UpdateExpenseAsync(Expense expense);
        Task<(bool Success, string Message)> DeleteExpenseAsync(int id);
    }
}
