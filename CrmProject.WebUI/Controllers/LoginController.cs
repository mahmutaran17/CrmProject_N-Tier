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
                return RedirectToAction("Index", "Home"); // Yetkisi yoksa Ana Sayfaya at
            }

            // ReturnUrl'i formda kullanabilmek için ViewBag'e atıyoruz
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string password, string ReturnUrl = null) // ReturnUrl eklendi
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
                    new Claim(ClaimTypes.Role, roleName)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // 1. ŞIK DOKUNUŞ: HOŞ GELDİN MESAJI
                TempData["Success"] = $"Sisteme hoş geldin, {user.FirstName}!";

                // 2. ŞIK DOKUNUŞ: RETURN URL MANTIĞI
                // Eğer kullanıcı belirli bir linke girmek isterken Login'e düştüyse, giriş yapınca o linke devam etsin
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }

                // Özel bir link yoksa direkt Dashboard'a (Ana Sayfaya) yönlendir.
                return RedirectToAction("Index", "Home");
            }

            // 3. ŞIK DOKUNUŞ: TOAST HATA MESAJI
            TempData["Error"] = "E-posta adresi veya şifre hatalı!";
            ViewBag.ReturnUrl = ReturnUrl; // Hata alsa bile gitmek istediği URL aklımızda kalsın
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // ÇIKIŞ MESAJI
            TempData["Success"] = "Sistemden güvenli bir şekilde çıkış yaptınız.";

            return RedirectToAction("Index");
        }
    }
}