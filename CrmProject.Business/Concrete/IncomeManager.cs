using CrmProject.Business.Abstract;
using CrmProject.DataAccess.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.Business.Concrete
{
    public class IncomeManager : GenericManager<Income>, IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IProjectRepository _projectRepository;

        public IncomeManager(IIncomeRepository incomeRepository, IProjectRepository projectRepository) : base(incomeRepository)
        {
            _incomeRepository = incomeRepository;
            _projectRepository = projectRepository;
        }

        public async Task<List<Income>> GetAllIncomesWithProjectAsync()
        {
            return await _incomeRepository.GetListWithIncludesAsync(null, x => x.Project);
        }

        public async Task<SelectList> GetActiveProjectsForDropdownAsync(int? selectedProjectId = null)
        {
            var projects = await _projectRepository.GetWhereAsync(x => x.Status == ProjectStatus.Aktif);
            return new SelectList(projects, "Id", "ProjectName", selectedProjectId);
        }

        public async Task<(bool Success, string Message)> CreateIncomeAsync(Income income)
        {
            await _incomeRepository.AddAsync(income);
            await _incomeRepository.SaveAsync();
            return (true, $"'{income.Description}' konulu Kazanç başarıyla eklendi.");
        }

        public async Task<(bool Success, string Message)> UpdateIncomeAsync(Income income)
        {
            _incomeRepository.Update(income);
            await _incomeRepository.SaveAsync();
            return (true, $"'{income.Description}' konulu gelir kaydı başarıyla güncellendi.");
        }

        public async Task<(bool Success, string Message)> DeleteIncomeAsync(int id)
        {
            var income = await _incomeRepository.GetByIdAsync(id);
            if (income == null)
                return (false, "Silinmek istenen gelir kaydı bulunamadı!");

            _incomeRepository.Delete(income);
            await _incomeRepository.SaveAsync();
            return (true, "Gelir kaydı sistemden başarıyla silindi.");
        }
    }
}