using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmProject.WebUI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var values = await _userService.GetActiveUsersAsync();
            return View(values);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser()
        {
            ViewBag.Roles = await _userService.GetRolesForDropdownAsync();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser(User user)
        {
            var result = await _userService.AddUserAsync(user);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.SoftDeleteUserAsync(id);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int id)
        {
            var userValue = await _userService.GetByIdAsync(id);
            ViewBag.Roles = await _userService.GetRolesForDropdownAsync(userValue?.RoleId);
            return View(userValue);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUser(User user, IFormFile? profileImage)
        {
            var result = await _userService.UpdateUserAsync(user, profileImage);
            TempData[result.Success ? "Success" : "Error"] = result.Message;

            var currentUserIdStr = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (currentUserIdStr == user.Id.ToString())
                return RedirectToAction("MyProfile");

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
            var currentUser = await _userService.GetMyProfileAsync(currentUserId);

            if (currentUser == null)
                return RedirectToAction("Index", "Login");

            return View(currentUser);
        }
    }
}