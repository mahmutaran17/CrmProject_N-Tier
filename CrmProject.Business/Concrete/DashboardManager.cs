using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;

namespace CrmProject.Business.Concrete
{
    public class DashboardManager : IDashboardService
    {
        private readonly IProjectService _projectService;
        private readonly IAppTaskService _appTaskService;
        private readonly IUserService _userService;
        private readonly IIncomeService _incomeService;
        private readonly IExpenseService _expenseService;

        public DashboardManager(
            IProjectService projectService,
            IAppTaskService appTaskService,
            IUserService userService,
            IIncomeService incomeService,
            IExpenseService expenseService)
        {
            _projectService = projectService;
            _appTaskService = appTaskService;
            _userService = userService;
            _incomeService = incomeService;
            _expenseService = expenseService;
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var projects = await _projectService.GetAllAsync();
            var tasks = await _appTaskService.GetTasksByUserRoleAsync(0, true);
            var taskList = tasks.ToList();
            var users = await _userService.GetActiveUsersAsync();
            var incomes = await _incomeService.GetAllAsync();
            var expenses = await _expenseService.GetAllAsync();

            return new DashboardDto
            {
                TotalProjects = projects.Count,
                TotalTasks = taskList.Count,
                ActiveUsers = users.Count,
                PendingTasks = taskList.Count(x => x.Status != AppTaskStatus.Tamamlandi),
                TotalIncome = incomes.Sum(x => x.Amount),
                TotalExpense = expenses.Sum(x => x.Amount),
                LastTasks = taskList.OrderByDescending(x => x.Id).Take(5).ToList()
            };
        }
    }
}
