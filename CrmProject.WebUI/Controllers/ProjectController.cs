using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmProject.WebUI.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        //letting system know we will talk to Services
        private readonly IProjectService _projectService;
        private readonly ICustomerService _customerService;
        private readonly IAppTaskService _appTaskService;
        private readonly IIncomeService _incomeService;
        private readonly IExpenseService _expenseService;

        //DI
        public ProjectController(IProjectService projectService, ICustomerService customerService, IIncomeService incomeService, IExpenseService expensenseService, IAppTaskService appTaskService)
        {
            _projectService = projectService;
            _customerService = customerService;
            _expenseService = expensenseService;
            _incomeService = incomeService;
            _appTaskService = appTaskService;
        }
        // view in project
        public async Task<IActionResult> Index()
        {
            // Artık projeleri çekerken y => y.Customer diyerek Müşteri tablosunu da joinliyoruz
            var values = await _projectService.GetListWithIncludesAsync(
                x => x.Status != ProjectStatus.Silindi,
                y => y.Customer
            );

            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> AddProject()
        {
            ViewBag.Customers = await _customerService.GetWhereAsync(x => x.Status == true);
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

        [Authorize(Roles = "Admin")]
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
            if (!User.IsInRole("Admin") && project.Status == ProjectStatus.Silindi)
            {
                project.Status = ProjectStatus.Aktif;
            }

            _projectService.Update(project);
            await _projectService.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null) return NotFound();

            // Projeye ait görevler, gelirler ve giderler
            ViewBag.Tasks = await _appTaskService.GetListWithIncludesAsync(x => x.ProjectId == id, y => y.AssignedUsers);
            ViewBag.Incomes = await _incomeService.GetWhereAsync(x => x.ProjectId == id);
            ViewBag.Expenses = await _expenseService.GetWhereAsync(x => x.ProjectId == id);

            // Kalan Bütçe Hesaplaması
            var totalIncome = ((IEnumerable<Income>)ViewBag.Incomes).Sum(x => x.Amount);
            var totalExpense = ((IEnumerable<Expense>)ViewBag.Expenses).Sum(x => x.Amount);
            ViewBag.Balance = totalIncome - totalExpense;

            return View(project);
        }


    }
}
