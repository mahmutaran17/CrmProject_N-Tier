using CrmProject.Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmProject.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _dashboardService.GetDashboardDataAsync();

            ViewBag.TotalProjects = data.TotalProjects;
            ViewBag.TotalTasks = data.TotalTasks;
            ViewBag.ActiveUsers = data.ActiveUsers;
            ViewBag.PendingTasks = data.PendingTasks;
            ViewBag.TotalIncome = data.TotalIncome;
            ViewBag.TotalExpense = data.TotalExpense;

            return View(data.LastTasks);
        }
    }
}
