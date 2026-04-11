using CrmProject.DataAccess.Abstract;
using CrmProject.DataAccess.Context;
using CrmProject.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CrmProject.Business.Concrete
{
    /// <summary>
    /// GAP-B3: Background service that runs once daily.
    /// Checks for active tasks with DueDate within the next 48 hours
    /// and creates warning notifications for all assigned users.
    /// </summary>
    public class DueDateWarningService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DueDateWarningService> _logger;

        // Run interval: once every 24 hours
        private static readonly TimeSpan Interval = TimeSpan.FromHours(24);

        // Warning threshold: tasks due within the next 48 hours
        private static readonly TimeSpan WarningThreshold = TimeSpan.FromHours(48);

        public DueDateWarningService(IServiceScopeFactory scopeFactory, ILogger<DueDateWarningService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DueDateWarningService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckAndNotifyAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "DueDateWarningService encountered an error.");
                }

                await Task.Delay(Interval, stoppingToken);
            }

            _logger.LogInformation("DueDateWarningService stopped.");
        }

        private async Task CheckAndNotifyAsync(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CrmDbContext>();

            var now = DateTime.Now;
            var threshold = now.Add(WarningThreshold);

            // Query: Active tasks (not completed) with a DueDate within the next 48 hours
            var urgentTasks = await context.AppTasks
                .Include(t => t.AssignedUsers)
                .Where(t => t.Status != AppTaskStatus.Tamamlandi
                         && t.Status != AppTaskStatus.IptalEdildi
                         && t.DueDate.HasValue
                         && t.DueDate.Value >= now
                         && t.DueDate.Value <= threshold)
                .ToListAsync(ct);

            if (!urgentTasks.Any())
            {
                _logger.LogInformation("DueDateWarningService: No tasks approaching deadline.");
                return;
            }

            int notificationCount = 0;

            foreach (var task in urgentTasks)
            {
                if (task.AssignedUsers == null || !task.AssignedUsers.Any())
                    continue;

                var hoursRemaining = (task.DueDate!.Value - now).TotalHours;
                var timeText = hoursRemaining < 24
                    ? $"{(int)hoursRemaining} saat"
                    : $"{(int)(hoursRemaining / 24)} gün {(int)(hoursRemaining % 24)} saat";

                foreach (var user in task.AssignedUsers)
                {
                    // Prevent duplicate notifications: check if we already sent one today
                    var alreadySent = await context.Notifications.AnyAsync(n =>
                        n.UserId == user.Id
                        && n.Message.Contains(task.Title)
                        && n.Message.Contains("teslim tarihi")
                        && n.CreatedAt.Date == now.Date, ct);

                    if (alreadySent)
                        continue;

                    var notification = new Notification
                    {
                        UserId = user.Id,
                        Message = $"⚠️ '{task.Title}' görevinin teslim tarihi yaklaşıyor! Kalan süre: {timeText}.",
                        IsRead = false,
                        CreatedAt = now
                    };

                    await context.Notifications.AddAsync(notification, ct);
                    notificationCount++;
                }
            }

            if (notificationCount > 0)
            {
                await context.SaveChangesAsync(ct);
                _logger.LogInformation("DueDateWarningService: {Count} deadline warning(s) sent.", notificationCount);
            }
        }
    }
}
