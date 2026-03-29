using CrmProject.Business.Abstract;
using CrmProject.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CrmProject.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IAppTaskService _appTaskService;
        private readonly IUserService _userService;
        private readonly IIncomeService _incomeService;
        private readonly IExpenseService _expenseService;
        public HomeController(IProjectService projectService, IAppTaskService appTaskService, IUserService userService, IIncomeService incomeService, IExpenseService expensenseService)
        {
            _projectService = projectService;
            _appTaskService = appTaskService;
            _userService = userService;
            _expenseService = expensenseService;
            _incomeService = incomeService;
        }

        public async Task<IActionResult> Index()
        {
            // İstatistikleri çekelim
            var projects = await _projectService.GetAllAsync();
            var tasks = await _appTaskService.GetAllAsync();
            var users = await _userService.GetWhereAsync(x => x.IsActive);
            var incomes = await _incomeService.GetAllAsync();
            var expenses = await _expenseService.GetAllAsync();

            ViewBag.TotalProjects = projects.Count;
            ViewBag.TotalTasks = tasks.Count;
            ViewBag.ActiveUsers = users.Count;
            ViewBag.PendingTasks = tasks.Count(x => x.Status != Entity.Entities.AppTaskStatus.Tamamlandi);
            ViewBag.TotalIncome = incomes.Sum(x => x.Amount);
            ViewBag.TotalExpense = expenses.Sum(x => x.Amount);

            // Son eklenen 5 görevi listeye gönderelim
            var lastTasks = tasks.OrderByDescending(x => x.Id).Take(5).ToList();

            return View(lastTasks);
        }
    }
}
