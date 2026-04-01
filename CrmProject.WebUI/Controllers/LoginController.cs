using System.Security.Claims;
using CrmProject.Business.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CrmProject.WebUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userServices;
        private readonly IRoleService _roleService;
        public LoginController(IUserService userService, IRoleService roleService)
        {
            _userServices = userService;
            _roleService = roleService;
        }
        [HttpGet]
        public IActionResult Index(string ReturnUrl = null)
        {
            // Kullanıcı zaten giriş yapmışsa ama yetkisiz bir linke tıkladıysa:
            if (User.Identity.IsAuthenticated)
            {
                TempData["Error"] = "Bu sayfaya veya işleme erişim yetkiniz bulunmamaktadır!";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string password)
        {
            var user = (await _userServices.GetWhereAsync(x => x.Email == email && x.PasswordHash == password && x.IsActive)).FirstOrDefault();

            if (user != null)
            {
                var role = await _roleService.GetByIdAsync(user.RoleId);
                string roleName = role != null ? role.RoleName : "Personel";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("UserId", user.Id.ToString()),
                    
                    // 4. SİHİRLİ SATIR: ROLÜ SİSTEME TANITIYORUZ!
                    new Claim(ClaimTypes.Role, roleName)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // cookie login
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "AppTask");
            }

            ViewBag.Error = "Eposta veya şifre hatalı !";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}
