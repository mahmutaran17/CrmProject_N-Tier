using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmProject.WebUI.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _projectService.GetActiveProjectsAsync();
            return View(values);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProject()
        {
            ViewBag.Customers = await _projectService.GetActiveCustomersForDropdownAsync();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProject(Project project)
        {
            var result = await _projectService.AddProjectAsync(project);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var result = await _projectService.SoftDeleteProjectAsync(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProject(int id)
        {
            var value = await _projectService.GetByIdAsync(id);
            if (value == null) return NotFound();

            ViewBag.Customers = await _projectService.GetActiveCustomersForDropdownAsync(value.CustomerId);
            return View(value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProject(Project project)
        {
            var result = await _projectService.UpdateProjectAsync(project);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var details = await _projectService.GetProjectDetailsAsync(id);
            if (details == null) return NotFound();

            ViewBag.Tasks = details.Value.Tasks;
            ViewBag.Incomes = details.Value.Incomes;
            ViewBag.Expenses = details.Value.Expenses;
            ViewBag.Balance = details.Value.Balance;

            return View(details.Value.Project);
        }
    }
}