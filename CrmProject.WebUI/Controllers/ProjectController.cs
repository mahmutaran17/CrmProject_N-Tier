using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // SelectList için gerekli
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrmProject.WebUI.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ICustomerService _customerService;
        private readonly IAppTaskService _appTaskService;
        private readonly IIncomeService _incomeService;
        private readonly IExpenseService _expenseService;

        public ProjectController(IProjectService projectService, ICustomerService customerService, IIncomeService incomeService, IExpenseService expenseService, IAppTaskService appTaskService)
        {
            _projectService = projectService;
            _customerService = customerService;
            _expenseService = expenseService;
            _incomeService = incomeService;
            _appTaskService = appTaskService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _projectService.GetListWithIncludesAsync(
                x => x.Status != ProjectStatus.Silindi,
                y => y.Customer
            );
            return View(values);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Proje ekleme işini sadece admin yapabilsin
        public async Task<IActionResult> AddProject()
        {
            // Dropdown için ViewBag yapısı düzeltildi
            var customers = await _customerService.GetWhereAsync(x => x.Status == true);
            ViewBag.Customers = new SelectList(customers, "Id", "CustomerName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProject(Project project)
        {
            project.Status = ProjectStatus.Aktif;
            await _projectService.AddAsync(project);
            await _projectService.SaveAsync();

            // BAŞARI MESAJI
            TempData["Success"] = $"'{project.ProjectName}' isimli proje başarıyla başlatıldı.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var value = await _projectService.GetByIdAsync(id);
            if (value != null)
            {
                value.Status = ProjectStatus.Silindi;
                _projectService.Update(value);
                await _projectService.SaveAsync();

                // BAŞARI MESAJI
                TempData["Success"] = "Proje sistemden silindi.";
            }
            else
            {
                TempData["Error"] = "Silinmek istenen proje bulunamadı!";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Proje güncelleme işini sadece admin yapabilsin
        public async Task<IActionResult> UpdateProject(int id)
        {
            var value = await _projectService.GetByIdAsync(id);
            if (value == null) return NotFound();

            // Dropdown için müşterileri tekrar çekip mevcut olanı (value.CustomerId) seçili yapıyoruz
            var customers = await _customerService.GetWhereAsync(x => x.Status == true);
            ViewBag.Customers = new SelectList(customers, "Id", "CustomerName", value.CustomerId);

            return View(value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProject(Project project)
        {
            // Eğer silinmiş bir proje yanlışlıkla güncelleniyorsa Aktife çek
            if (project.Status == ProjectStatus.Silindi)
            {
                project.Status = ProjectStatus.Aktif;
            }

            _projectService.Update(project);
            await _projectService.SaveAsync();

            // BAŞARI MESAJI
            TempData["Success"] = $"'{project.ProjectName}' projesi başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null) return NotFound();

            ViewBag.Tasks = await _appTaskService.GetListWithIncludesAsync(x => x.ProjectId == id, y => y.AssignedUsers);
            ViewBag.Incomes = await _incomeService.GetWhereAsync(x => x.ProjectId == id);
            ViewBag.Expenses = await _expenseService.GetWhereAsync(x => x.ProjectId == id);

            var totalIncome = ((IEnumerable<Income>)ViewBag.Incomes).Sum(x => x.Amount);
            var totalExpense = ((IEnumerable<Expense>)ViewBag.Expenses).Sum(x => x.Amount);
            ViewBag.Balance = totalIncome - totalExpense;

            return View(project);
        }
    }
}