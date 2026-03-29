using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.WebUI.Controllers
{
    [Authorize]
    public class AppTaskController : Controller
    {
        private readonly IAppTaskService _appTaskService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public AppTaskController(IAppTaskService appTaskService, IProjectService projectService, IUserService userService)
        {
            _appTaskService = appTaskService;
            _projectService = projectService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _appTaskService.GetListWithIncludesAsync(null, x => x.Project);
            return View(tasks);
        }

        // --- GÖREV EKLEME (CREATE) ---
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var projects = await _projectService.GetWhereAsync(x => x.Status == ProjectStatus.Aktif);
            ViewBag.Projects = new SelectList(projects, "Id", "ProjectName");

            var users = await _userService.GetWhereAsync(x => x.IsActive);
            ViewBag.Users = new SelectList(users, "Id", "FirstName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppTask task, List<int> SelectedUserIds)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userIdClaim != null)
            {
                task.AssignedByUserId = int.Parse(userIdClaim);
            }

            if (SelectedUserIds != null)
            {
                foreach (var userId in SelectedUserIds)
                {
                    var user = await _userService.GetByIdAsync(userId);
                    if (user != null) task.AssignedUsers.Add(user);
                }
            }

            await _appTaskService.AddAsync(task);
            await _appTaskService.SaveAsync();
            return RedirectToAction("Index");
        }

        // --- GÖREV GÜNCELLEME (UPDATE) ---
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var task = await _appTaskService.GetByIdAsync(id);

            var projects = await _projectService.GetWhereAsync(x => x.Status == ProjectStatus.Aktif);
            ViewBag.Projects = new SelectList(projects, "Id", "ProjectName");

            var users = await _userService.GetWhereAsync(x => x.IsActive);
            ViewBag.Users = new SelectList(users, "Id", "FirstName");

            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AppTask task, List<int> SelectedUserIds)
        {
            // Görev bilgileri güncelleniyor
            _appTaskService.Update(task);
            await _appTaskService.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}