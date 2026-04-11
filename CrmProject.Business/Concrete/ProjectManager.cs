using CrmProject.Business.Abstract;
using CrmProject.DataAccess.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.Business.Concrete
{
    public class ProjectManager : GenericManager<Project>, IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAppTaskRepository _appTaskRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IExpenseRepository _expenseRepository;

        public ProjectManager(
            IProjectRepository projectRepository,
            ICustomerRepository customerRepository,
            IAppTaskRepository appTaskRepository,
            IIncomeRepository incomeRepository,
            IExpenseRepository expenseRepository) : base(projectRepository)
        {
            _projectRepository = projectRepository;
            _customerRepository = customerRepository;
            _appTaskRepository = appTaskRepository;
            _incomeRepository = incomeRepository;
            _expenseRepository = expenseRepository;
        }

        public async Task<List<Project>> GetActiveProjectsAsync()
        {
            return await _projectRepository.GetListWithIncludesAsync(
                x => x.Status != ProjectStatus.Silindi,
                y => y.Customer);
        }

        public async Task<SelectList> GetActiveCustomersForDropdownAsync(int? selectedCustomerId = null)
        {
            var customers = await _customerRepository.GetWhereAsync(x => x.Status == true);
            return new SelectList(customers, "Id", "CustomerName", selectedCustomerId);
        }

        public async Task<(bool Success, string Message)> AddProjectAsync(Project project)
        {
            project.Status = ProjectStatus.Aktif;
            await _projectRepository.AddAsync(project);
            await _projectRepository.SaveAsync();
            return (true, $"'{project.ProjectName}' isimli proje başarıyla başlatıldı.");
        }

        public async Task<(bool Success, string Message)> UpdateProjectAsync(Project project)
        {
            if (project.Status == ProjectStatus.Silindi)
                project.Status = ProjectStatus.Aktif;

            _projectRepository.Update(project);
            await _projectRepository.SaveAsync();
            return (true, $"'{project.ProjectName}' projesi başarıyla güncellendi.");
        }

        public async Task<(bool Success, string Message)> SoftDeleteProjectAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                return (false, "Silinmek istenen proje bulunamadı!");

            project.Status = ProjectStatus.Silindi;
            _projectRepository.Update(project);
            await _projectRepository.SaveAsync();
            return (true, "Proje sistemden silindi.");
        }

        public async Task<(Project Project, List<AppTask> Tasks, List<Income> Incomes, List<Expense> Expenses, decimal Balance)?> GetProjectDetailsAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return null;

            var tasks = await _appTaskRepository.GetListWithIncludesAsync(x => x.ProjectId == id, y => y.AssignedUsers);
            var incomes = await _incomeRepository.GetWhereAsync(x => x.ProjectId == id);
            var expenses = await _expenseRepository.GetWhereAsync(x => x.ProjectId == id);

            var totalIncome = incomes.Sum(x => x.Amount);
            var totalExpense = expenses.Sum(x => x.Amount);
            var balance = totalIncome - totalExpense;

            return (project, tasks, incomes, expenses, balance);
        }
    }
}
