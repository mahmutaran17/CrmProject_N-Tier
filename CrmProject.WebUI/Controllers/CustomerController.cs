using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CrmProject.WebUI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _customerService.GetActiveCustomersAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult AddCustomer() => View();

        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            var result = await _customerService.AddCustomerAsync(customer);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            var value = await _customerService.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(Customer customer)
        {
            var result = await _customerService.UpdateCustomerAsync(customer);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var result = await _customerService.SoftDeleteCustomerAsync(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }
    }
}