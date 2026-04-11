using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmProject.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        public async Task<IActionResult> Index()
        {
            var expenses = await _expenseService.GetAllExpensesWithProjectAsync();
            return View(expenses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Projects = await _expenseService.GetActiveProjectsForDropdownAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            var result = await _expenseService.CreateExpenseAsync(expense);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var expense = await _expenseService.GetByIdAsync(id);
            if (expense == null) return NotFound();

            ViewBag.Projects = await _expenseService.GetActiveProjectsForDropdownAsync(expense.ProjectId);
            return View(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Expense expense)
        {
            var result = await _expenseService.UpdateExpenseAsync(expense);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _expenseService.DeleteExpenseAsync(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }
    }
}
