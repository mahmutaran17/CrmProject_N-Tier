using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.WebUI.Controllers
{
    public class IncomeController : Controller
    {
        private readonly IIncomeService _incomeService;
        private readonly IProjectService _projectService;

        public IncomeController(IIncomeService incomeService, IProjectService projectService)
        {
            _incomeService = incomeService;
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var incomes = await _incomeService.GetListWithIncludesAsync(null, x => x.Project);
            return View(incomes);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var projects = await _projectService.GetWhereAsync(x => x.Status == ProjectStatus.Aktif);
            ViewBag.Projects = new SelectList(projects, "Id", "ProjectName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Income income)
        {
            await _incomeService.AddAsync(income);
            await _incomeService.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}