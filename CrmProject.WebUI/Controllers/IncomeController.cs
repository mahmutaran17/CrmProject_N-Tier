using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class IncomeController : Controller
    {

        private readonly IIncomeService _incomeService;
        private readonly IProjectService _projectService;

        public IncomeController(IIncomeService incomeService, IProjectService projectService)
        {
            _incomeService = incomeService;
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var incomes = await _incomeService.GetListWithIncludesAsync(null, x => x.Project);
            return View(incomes);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var projects = await _projectService.GetWhereAsync(x => x.Status == ProjectStatus.Aktif);
            ViewBag.Projects = new SelectList(projects, "Id", "ProjectName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Income income)
        {
            await _incomeService.AddAsync(income);
            await _incomeService.SaveAsync();
            TempData["Success"] = $"'{income.Description}' konulu Kazanç başarıyla eklendi .";
            return RedirectToAction("Index");
        }

        // --- GELİR GÜNCELLEME (GET) ---
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var income = await _incomeService.GetByIdAsync(id);
            if (income == null) return NotFound();

            // Dropdown için aktif projeleri tekrar gönderiyoruz (mevcut projeyi seçili hale getirerek)
            var projects = await _projectService.GetWhereAsync(x => x.Status == ProjectStatus.Aktif);
            ViewBag.Projects = new SelectList(projects, "Id", "ProjectName", income.ProjectId);

            return View(income);
        }

        // --- GELİR GÜNCELLEME (POST) ---
        [HttpPost]
        public async Task<IActionResult> Update(Income income)
        {
            _incomeService.Update(income);
            await _incomeService.SaveAsync();

            TempData["Success"] = $"'{income.Description}' konulu gelir kaydı başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        // --- GELİR SİLME (DELETE) ---
        public async Task<IActionResult> Delete(int id)
        {
            var income = await _incomeService.GetByIdAsync(id);
            if (income != null)
            {
                // Finansal kayıtlarda genelde tamamen silmek (Hard Delete) yerine 
                // Status = false (Soft Delete) yapılır. Eğer Income tablonda Status varsa aşağıdaki gibi yapabilirsin:
                // income.Status = false;
                // _incomeService.Update(income);

                // Eğer Status yoksa ve veritabanından kalıcı olarak silmek istiyorsan:
                _incomeService.Delete(income);

                await _incomeService.SaveAsync();
                TempData["Success"] = "Gelir kaydı sistemden başarıyla silindi.";
            }
            else
            {
                TempData["Error"] = "Silinmek istenen gelir kaydı bulunamadı!";
            }

            return RedirectToAction("Index");
        }
    }
}