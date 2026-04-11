using CrmProject.Business.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CrmProject.WebUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet]
        public IActionResult Index(string ReturnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData["Error"] = "Bu sayfaya veya işleme erişim yetkiniz bulunmamaktadır!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string password, string ReturnUrl = null)
        {
            var principal = await _loginService.AuthenticateAsync(email, password);

            if (principal != null)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                var userName = principal.Identity?.Name ?? "";
                TempData["Success"] = $"Sisteme hoş geldin, {userName.Split(' ')[0]}!";

                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    return Redirect(ReturnUrl);

                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "E-posta adresi veya şifre hatalı!";
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Success"] = "Sistemden güvenli bir şekilde çıkış yaptınız.";
            return RedirectToAction("Index");
        }
    }
}