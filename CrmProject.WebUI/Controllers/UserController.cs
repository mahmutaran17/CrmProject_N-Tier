using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        // Dependency Injection ile servisleri alıyoruz
        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        // --- KULLANICI LİSTESİ ---
        public async Task<IActionResult> Index()
        {
            // Sadece aktif kullanıcıları getir (Soft-Delete mantığına uygun)
            var values = await _userService.GetWhereAsync(x => x.IsActive == true);
            return View(values);
        }

        // --- YENİ KULLANICI EKLEME (GET) ---
        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            // Rolleri veritabanından çekip Dropdown (Seçim Kutusu) için hazırlıyoruz
            var roles = await _roleService.GetAllAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "RoleName");

            return View();
        }

        // --- YENİ KULLANICI EKLEME (POST) ---
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            // Sistemsel varsayılan değerleri arkaplanda biz atıyoruz
            user.IsActive = true;
            user.RegistrationDate = DateTime.Now;

            await _userService.AddAsync(user);
            await _userService.SaveAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var userValue = await _userService.GetByIdAsync(id);

            if(userValue != null)
            {
                userValue.IsActive = false;
                _userService.Update(userValue);
                await _userService.SaveAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id)
        {
            var userValue = await _userService.GetByIdAsync(id);

            var roles = await _roleService.GetAllAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "RoleName");

            return View(userValue);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(User user)
        {
            _userService.Update(user);
            await _userService.SaveAsync();

            return RedirectToAction("Index");
        }
    }
}