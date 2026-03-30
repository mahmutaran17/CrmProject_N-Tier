using CrmProject.Business.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.WebUI.Controllers
{
    [Authorize]
    public class AppTaskController : Controller
    {
        private readonly IAppTaskService _appTaskService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService; // EKLENDİ

        public AppTaskController(IAppTaskService appTaskService, IProjectService projectService, IUserService userService, INotificationService notificationService)
        {
            _appTaskService = appTaskService;
            _projectService = projectService;
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _appTaskService.GetListWithIncludesAsync(null, x => x.Project);
            return View(tasks);
        }

        // --- GÖREV EKLEME (CREATE) ---
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var projects = await _projectService.GetWhereAsync(x => x.Status == ProjectStatus.Aktif);
            ViewBag.Projects = new SelectList(projects, "Id", "ProjectName");

            var users = await _userService.GetWhereAsync(x => x.IsActive);
            ViewBag.Users = new SelectList(users, "Id", "FirstName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppTask task, List<int> SelectedUserIds)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userIdClaim != null)
            {
                task.AssignedByUserId = int.Parse(userIdClaim);
            }

            if (SelectedUserIds != null)
            {
                foreach (var userId in SelectedUserIds)
                {
                    var user = await _userService.GetByIdAsync(userId);
                    if (user != null) task.AssignedUsers.Add(user);
                }
            }

            // 1. Senin mevcut kodun: Görevi veritabanına kaydediyoruz
            await _appTaskService.AddAsync(task);
            await _appTaskService.SaveAsync();

            // 2. YENİ EKLENEN KISIM: Atanan personellere bildirim gönderiyoruz
            if (SelectedUserIds != null)
            {
                foreach (var userId in SelectedUserIds)
                {
                    var notification = new Notification
                    {
                        UserId = userId,
                        Message = $"'{task.Title}' başlıklı yeni bir görev size atandı.",
                        IsRead = false,
                        CreatedAt = DateTime.Now
                    };
                    // Controller'ın üst kısmında _notificationService'i DI ile projeye dahil ettiğini varsayıyorum
                    await _notificationService.AddAsync(notification);
                }
                await _notificationService.SaveAsync();
            }

            return RedirectToAction("Index");
        }

        // --- GÖREV GÜNCELLEME (UPDATE) ---
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var task = await _appTaskService.GetByIdAsync(id);

            var projects = await _projectService.GetWhereAsync(x => x.Status == ProjectStatus.Aktif);
            ViewBag.Projects = new SelectList(projects, "Id", "ProjectName");

            var users = await _userService.GetWhereAsync(x => x.IsActive);
            ViewBag.Users = new SelectList(users, "Id", "FirstName");

            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AppTask task, List<int> SelectedUserIds)
        {
            // Görev bilgileri güncelleniyor
            _appTaskService.Update(task);
            await _appTaskService.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}