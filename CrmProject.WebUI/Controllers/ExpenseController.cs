using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly IProjectService _projectService;


        public ExpenseController(IExpenseService expenseService, IProjectService projectService)
        {
            _expenseService = expenseService;
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var expenses = await _expenseService.GetListWithIncludesAsync(null, x => x.Project);
            return View(expenses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var projects = await _projectService.GetWhereAsync(x => x.Status == ProjectStatus.Aktif);
            ViewBag.Projects = new SelectList(projects, "Id", "ProjectName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            await _expenseService.AddAsync(expense);
            await _expenseService.SaveAsync();
            TempData["Success"] = $"'{expense.Description}' konulu Masraf başarıyla eklendi .";
            return RedirectToAction("Index");

        }

        // --- GİDER GÜNCELLEME (GET) ---
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var expense = await _expenseService.GetByIdAsync(id);
            if (expense == null) return NotFound();

            // Dropdown için projeleri tekrar gönderiyoruz
            var projects = await _projectService.GetWhereAsync(x => x.Status == ProjectStatus.Aktif);
            ViewBag.Projects = new SelectList(projects, "Id", "ProjectName", expense.ProjectId);

            return View(expense);
        }

        // --- GİDER GÜNCELLEME (POST) ---
        [HttpPost]
        public async Task<IActionResult> Update(Expense expense)
        {
            _expenseService.Update(expense);
            await _expenseService.SaveAsync();

            TempData["Success"] = $"'{expense.Description}' konulu masraf başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        // --- GİDER SİLME (DELETE) ---
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _expenseService.GetByIdAsync(id);
            if (expense != null)
            {
                // Finansal kayıtlarda genelde tamamen silmek (Hard Delete) yerine 
                // Status = false (Soft Delete) yapılır. Eğer Expense tablonda Status varsa aşağıdaki gibi yapabilirsin:
                // expense.Status = false;
                // _expenseService.Update(expense);

                // Eğer Status yoksa ve tamamen silmek istiyorsan Delete metodunu çağır:
                _expenseService.Delete(expense);

                await _expenseService.SaveAsync();
                TempData["Success"] = "Masraf kaydı sistemden silindi.";
            }
            else
            {
                TempData["Error"] = "Silinmek istenen masraf kaydı bulunamadı!";
            }

            return RedirectToAction("Index");
        }
    }
}
