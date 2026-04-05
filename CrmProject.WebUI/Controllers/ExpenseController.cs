using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly IProjectService _projectService;


        public ExpenseController(IExpenseService expenseService, IProjectService projectService)
        {
            _expenseService = expenseService;
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var expenses = await _expenseService.GetListWithIncludesAsync(null, x => x.Project);
            return View(expenses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var projects = await _projectService.GetWhereAsync(x => x.Status == ProjectStatus.Aktif);
            ViewBag.Projects = new SelectList(projects, "Id", "ProjectName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            await _expenseService.AddAsync(expense);
            await _expenseService.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
