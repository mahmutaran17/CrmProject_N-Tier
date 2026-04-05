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
            // HATA BURADAYDI: Sadece durumu True (Aktif) olanları getirmeliyiz!
            var values = await _customerService.GetWhereAsync(x => x.Status == true);
            return View(values);
        }

        [HttpGet]
        public IActionResult AddCustomer() => View();

        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            // Formdan ne gelirse gelsin, biz arka planda yeni müşteriyi banko Aktif yapalım (Güvenlik)
            customer.Status = true;

            await _customerService.AddAsync(customer);
            await _customerService.SaveAsync();

            TempData["Success"] = $"{customer.CustomerName} isimli müşteri başarıyla portföye eklendi. ";

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
            customer.Status = true;
            _customerService.Update(customer);
            await _customerService.SaveAsync();
            TempData["Success"] = $"{customer.CustomerName} isimli müşteri başarıyla güncellendi. ";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var value = await _customerService.GetByIdAsync(id);

            if (value != null)
            {
                value.Status = false; // Soft delete
                _customerService.Update(value);
                await _customerService.SaveAsync();

                TempData["Success"] = $"{value.CustomerName} isimli müşteri sistemden silindi.";
            }
            else
            {
                //id yanlış gelirse
                TempData["Error"] = "Silinmek istenen müşteri bulunamadı!";
            }

            return RedirectToAction("Index");
        }
    }
}