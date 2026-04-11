using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmProject.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class IncomeController : Controller
    {
        private readonly IIncomeService _incomeService;

        public IncomeController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        public async Task<IActionResult> Index()
        {
            var incomes = await _incomeService.GetAllIncomesWithProjectAsync();
            return View(incomes);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Projects = await _incomeService.GetActiveProjectsForDropdownAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Income income)
        {
            var result = await _incomeService.CreateIncomeAsync(income);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var income = await _incomeService.GetByIdAsync(id);
            if (income == null) return NotFound();

            ViewBag.Projects = await _incomeService.GetActiveProjectsForDropdownAsync(income.ProjectId);
            return View(income);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Income income)
        {
            var result = await _incomeService.UpdateIncomeAsync(income);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _incomeService.DeleteIncomeAsync(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }
    }
}