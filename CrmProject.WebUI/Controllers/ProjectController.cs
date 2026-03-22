using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CrmProject.WebUI.Controllers
{
    public class ProjectController : Controller
    {
        //letting system know we will talk to Services
        private readonly IProjectService _projectService;

        //DI
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        // view in project
        public async Task<IActionResult> Index()
        {
            var values = await _projectService.GetWhereAsync(x => x.Status != ProjectStatus.Silindi);

            return View(values);
        }

        [HttpGet]
        public IActionResult AddProject()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProject(Project project)
        {
            project.Status = ProjectStatus.Aktif;
            //filled form from view
            await _projectService.AddAsync(project);
            await _projectService.SaveAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProject(int id)
        {
            //bringing the id from db
            var value = await _projectService.GetByIdAsync(id);
            value.Status = ProjectStatus.Silindi;
            _projectService.Update(value);
            await _projectService.SaveAsync();
            return RedirectToAction("Index");
        }

        //when user engage the update button
        [HttpGet]
        public async Task<IActionResult> UpdateProject(int id)
        {
            var value = await _projectService.GetByIdAsync(id);
            return View(value);
        }

        //when user fill the form and hit update
        [HttpPost]
        public async Task<IActionResult> UpdateProject(Project project)
        {
            _projectService.Update(project);
            await _projectService.SaveAsync();
            return RedirectToAction("Index");
        }



    }
}
