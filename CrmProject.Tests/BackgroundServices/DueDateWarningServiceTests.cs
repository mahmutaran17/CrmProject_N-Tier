using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrmProject.Business.Concrete;
using CrmProject.DataAccess.Context;
using CrmProject.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CrmProject.Tests.BackgroundServices
{
    public class DueDateWarningServiceTests
    {
        [Fact]
        public async Task ExecuteAsync_SuccessfullyFetchesTasks_ExpiringWithinNext48Hours()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CrmDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var initContext = new CrmDbContext(options))
            {
                var user = new User { Id = 1, FirstName = "Test", LastName = "User", Email = "test@user.com", PasswordHash = "x", RoleId = 1 };
                var assigner = new User { Id = 2, FirstName = "Admin", LastName = "User", Email = "admin@user.com", PasswordHash = "x", RoleId = 1 };
                
                initContext.Users.AddRange(user, assigner);

                // Task due within 48 hours (should generate warning)
                var urgentTask = new AppTask
                {
                    Id = 1,
                    Title = "Acil Görev",
                    ProjectId = 1,
                    AssignedByUserId = 2,
                    Status = AppTaskStatus.Beklemede,
                    DueDate = DateTime.Now.AddHours(24) // Inside 48h limit
                };
                urgentTask.AssignedUsers.Add(user);

                // Task due way in the future (should be ignored)
                var futureTask = new AppTask
                {
                    Id = 2,
                    Title = "Uzak Görev",
                    ProjectId = 1,
                    AssignedByUserId = 2,
                    Status = AppTaskStatus.Beklemede,
                    DueDate = DateTime.Now.AddDays(10) // Outside limit
                };
                futureTask.AssignedUsers.Add(user);

                initContext.AppTasks.AddRange(urgentTask, futureTask);
                await initContext.SaveChangesAsync();
            }

            // Mock IServiceScopeFactory returning an injected InMemory CrmDbContext
            var serviceProvider = new ServiceCollection()
                .AddScoped(sp => new CrmDbContext(options))
                .BuildServiceProvider();

            var mockScopeFactory = new Mock<IServiceScopeFactory>();
            var mockScope = new Mock<IServiceScope>();
            mockScope.Setup(s => s.ServiceProvider).Returns(serviceProvider);
            mockScopeFactory.Setup(f => f.CreateScope()).Returns(mockScope.Object);

            var mockLogger = new Mock<ILogger<DueDateWarningService>>();

            var service = new DueDateWarningService(mockScopeFactory.Object, mockLogger.Object);
            var hostedService = service as IHostedService;

            var cts = new CancellationTokenSource();
            
            // Act
            await hostedService.StartAsync(cts.Token);
            
            // Allow background loop to process its first iteration
            await Task.Delay(500); 
            cts.Cancel(); // Ensure loop exits
            
            // Assert
            using (var verifyContext = new CrmDbContext(options))
            {
                var notifications = await verifyContext.Notifications.ToListAsync();
                
                // Assert that only ONE notification was generated (for the 24h task)
                Assert.Single(notifications);
                Assert.Contains("Acil Görev", notifications.First().Message);
                Assert.Contains("Tarihi yaklaşıyor", notifications.First().Message);
            }
        }
    }
}
