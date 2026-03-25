using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.WebUI.Controllers
{
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
            var values = await _appTaskService.GetAllAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var projects = await _projectService.GetAllAsync();
            ViewBag.Projects = new SelectList(projects, "Id", "ProjectName");

            var users = await _userService.GetAllAsync();
            ViewBag.Users = new SelectList(users.Select(x => new { Id = x.Id, FullName = x.FirstName + " " + x.LastName }), "Id", "FullName");

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(AppTask appTask)
        {
            await _appTaskService.AddAsync(appTask);
            return RedirectToAction("Index");
        }
    }
}
