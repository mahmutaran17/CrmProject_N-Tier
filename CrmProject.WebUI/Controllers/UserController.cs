using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.WebUI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            // Sadece aktif kullanıcıları getir (Soft-Delete mantığına uygun)
            var values = await _userService.GetWhereAsync(x => x.IsActive == true);
            return View(values);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser()
        {
            // Rolleri veritabanından çekip Dropdown (Seçim Kutusu) için hazırlıyoruz
            var roles = await _roleService.GetAllAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "RoleName");

            return View();
        }

        // --- YENİ KULLANICI EKLEME (POST) ---
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser(User user)
        {
            // Sistemsel varsayılan değerleri arkaplanda biz atıyoruz
            user.IsActive = true;
            user.RegistrationDate = DateTime.Now;

            await _userService.AddAsync(user);
            await _userService.SaveAsync();
            TempData["Success"] = $"Kullanici Başarıyla Eklendi. ";
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userValue = await _userService.GetByIdAsync(id);

            if(userValue != null)
            {
                userValue.IsActive = false;
                _userService.Update(userValue);
                await _userService.SaveAsync();

                TempData["Success"] = $"{userValue.FirstName} {userValue.LastName} isimli personel sistemden silindi.";
            }

            else
            {
                //id yanlış gelirse
                TempData["Error"] = "Silinmek istenen personel bulunamadı!";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int id)
        {
            var userValue = await _userService.GetByIdAsync(id);

            var roles = await _roleService.GetAllAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "RoleName");

            return View(userValue);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUser(User user, IFormFile? profileImage)
        {
            // Veritabanındaki eski kullanıcıyı bul
            var existingUser = await _userService.GetByIdAsync(user.Id);

            if (existingUser != null)
            {
                // Temel bilgileri güncelle
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                existingUser.RoleId = user.RoleId;
                existingUser.IsActive = user.IsActive;

                // Şifre kutusu doluysa şifreyi güncelle, boşsa eski şifrede bırak
                if (!string.IsNullOrWhiteSpace(user.PasswordHash))
                {
                    existingUser.PasswordHash = user.PasswordHash;
                }

                // --- FOTOĞRAF YÜKLEME KISMI ---
                if (profileImage != null && profileImage.Length > 0)
                {
                    var extension = Path.GetExtension(profileImage.FileName);
                    var newImageName = Guid.NewGuid().ToString() + extension;
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var saveLocation = Path.Combine(folderPath, newImageName);

                    using (var stream = new FileStream(saveLocation, FileMode.Create))
                    {
                        await profileImage.CopyToAsync(stream);
                    }

                    // Fotoğrafın adını veritabanına kaydet
                    existingUser.ProfileImageUrl = "/images/profiles/" + newImageName;
                }

                _userService.Update(existingUser);
                await _userService.SaveAsync();

                TempData["Success"] = "Profil başarıyla güncellendi.";
            }

            // Giriş yapan kişi ile güncellenen kişi aynıysa kendi profiline dönsün
            var currentUserIdStr = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (currentUserIdStr == user.Id.ToString())
            {
                return RedirectToAction("MyProfile");
            }

            // Admin başka bir personeli güncellediyse listeye dönsün
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Index", "Login");

            int currentUserId = int.Parse(userIdStr);
            var currentUser = await _userService.GetByIdAsync(currentUserId);

            if (currentUser == null)
                return RedirectToAction("Index", "Login");

            

            return View(currentUser);
        }
    }
}