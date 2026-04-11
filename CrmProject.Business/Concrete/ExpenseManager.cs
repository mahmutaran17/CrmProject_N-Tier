using CrmProject.Business.Abstract;
using CrmProject.DataAccess.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.Business.Concrete
{
    public class ExpenseManager : GenericManager<Expense>, IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IProjectRepository _projectRepository;

        public ExpenseManager(IExpenseRepository expenseRepository, IProjectRepository projectRepository) : base(expenseRepository)
        {
            _expenseRepository = expenseRepository;
            _projectRepository = projectRepository;
        }

        public async Task<List<Expense>> GetAllExpensesWithProjectAsync()
        {
            return await _expenseRepository.GetListWithIncludesAsync(null, x => x.Project);
        }

        public async Task<SelectList> GetActiveProjectsForDropdownAsync(int? selectedProjectId = null)
        {
            var projects = await _projectRepository.GetWhereAsync(x => x.Status == ProjectStatus.Aktif);
            return new SelectList(projects, "Id", "ProjectName", selectedProjectId);
        }

        public async Task<(bool Success, string Message)> CreateExpenseAsync(Expense expense)
        {
            await _expenseRepository.AddAsync(expense);
            await _expenseRepository.SaveAsync();
            return (true, $"'{expense.Description}' konulu Masraf başarıyla eklendi.");
        }

        public async Task<(bool Success, string Message)> UpdateExpenseAsync(Expense expense)
        {
            _expenseRepository.Update(expense);
            await _expenseRepository.SaveAsync();
            return (true, $"'{expense.Description}' konulu masraf başarıyla güncellendi.");
        }

        public async Task<(bool Success, string Message)> DeleteExpenseAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null)
                return (false, "Silinmek istenen masraf kaydı bulunamadı!");

            _expenseRepository.Delete(expense);
            await _expenseRepository.SaveAsync();
            return (true, "Masraf kaydı sistemden silindi.");
        }
    }
}
