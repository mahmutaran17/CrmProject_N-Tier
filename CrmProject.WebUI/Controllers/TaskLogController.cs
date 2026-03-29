using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.WebUI.Controllers
{
    [Authorize]
    public class TaskLogController : Controller
    {
        private readonly ITaskLogService _taskLogService;
        private readonly IAppTaskService _appTaskService;

        public TaskLogController(ITaskLogService taskLogService, IAppTaskService appTaskService)
        {
            _taskLogService = taskLogService;
            _appTaskService = appTaskService;
        }

        public async Task<IActionResult> Index()
        {
            // Senin nav property ismin 'TaskItem' olduğu için onu Include ediyoruz
            var logs = await _taskLogService.GetListWithIncludesAsync(null, x => x.TaskItem);
            return View(logs);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? taskId)
        {
            var tasks = await _appTaskService.GetAllAsync();
            ViewBag.Tasks = new SelectList(tasks, "Id", "Title", taskId);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskLog taskLog)
        {
            // Senin entity'ndeki tarih property'si
            taskLog.CreatedAt = System.DateTime.Now;

            await _taskLogService.AddAsync(taskLog);
            await _taskLogService.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}