using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmProject.WebUI.Controllers
{
    [Authorize]
    public class TaskLogController : Controller
    {
        private readonly ITaskLogService _taskLogService;

        public TaskLogController(ITaskLogService taskLogService)
        {
            _taskLogService = taskLogService;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _taskLogService.GetAllLogsWithTaskAsync();
            return View(logs);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? taskId)
        {
            ViewBag.Tasks = await _taskLogService.GetTasksForDropdownAsync(taskId);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskLog taskLog)
        {
            var result = await _taskLogService.CreateLogAsync(taskLog);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Details", "AppTask", new { id = result.TaskItemId });
        }
    }
}