using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmProject.WebUI.Controllers
{
    [Authorize]
    public class AppTaskController : Controller
    {
        private readonly IAppTaskService _appTaskService;

        public AppTaskController(IAppTaskService appTaskService)
        {
            _appTaskService = appTaskService;
        }

        public async Task<IActionResult> Index()
        {
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            int currentUserId = string.IsNullOrEmpty(userIdStr) ? 0 : int.Parse(userIdStr);
            bool isAdmin = User.IsInRole("Admin");

            var tasks = await _appTaskService.GetTasksByUserRoleAsync(currentUserId, isAdmin);
            return View(tasks);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var task = await _appTaskService.GetTaskDetailsByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? projectId)
        {
            ViewBag.Projects = await _appTaskService.GetActiveProjectsForDropdownAsync(projectId);
            ViewBag.Users = await _appTaskService.GetActiveUsersForDropdownAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppTask task, List<int> SelectedUserIds)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            int assignedByUserId = userIdClaim != null ? int.Parse(userIdClaim) : 0;

            var result = await _appTaskService.CreateTaskWithRelationsAsync(task, SelectedUserIds, assignedByUserId);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var task = await _appTaskService.GetTaskDetailsByIdAsync(id);
            if (task == null) return NotFound();

            ViewBag.Projects = await _appTaskService.GetActiveProjectsForDropdownAsync(task.ProjectId);
            ViewBag.Users = await _appTaskService.GetActiveUsersForDropdownAsync();
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AppTask task, List<int> SelectedUserIds)
        {
            var result = await _appTaskService.UpdateTaskWithRelationsAsync(task, SelectedUserIds);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, AppTaskStatus status)
        {
            var result = await _appTaskService.UpdateTaskStatusAsync(id, status);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Details", new { id });
        }
    }
}