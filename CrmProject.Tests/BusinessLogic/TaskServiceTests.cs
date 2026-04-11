using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrmProject.Business.Concrete;
using CrmProject.Business.Abstract;
using CrmProject.DataAccess.Abstract;
using CrmProject.Entity.Entities;
using Moq;
using Xunit;

namespace CrmProject.Tests.BusinessLogic
{
    public class TaskServiceTests
    {
        private readonly Mock<IAppTaskRepository> _mockTaskRepo;
        private readonly Mock<IGenericRepository<User>> _mockUserRepo;
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly Mock<IGenericRepository<Project>> _mockProjectRepo;
        private readonly AppTaskManager _taskManager;

        public TaskServiceTests()
        {
            _mockTaskRepo = new Mock<IAppTaskRepository>();
            _mockUserRepo = new Mock<IGenericRepository<User>>();
            _mockNotificationService = new Mock<INotificationService>();
            _mockProjectRepo = new Mock<IGenericRepository<Project>>();

            _taskManager = new AppTaskManager(
                _mockTaskRepo.Object,
                _mockUserRepo.Object,
                _mockNotificationService.Object,
                _mockProjectRepo.Object);
        }

        [Fact]
        public async Task CreateTaskWithRelationsAsync_ThrowsExceptionWithGecmis_IfDueDateIsInPast()
        {
            // Arrange
            var task = new AppTask
            {
                Title = "Geçmiş Görev",
                DueDate = DateTime.Now.AddDays(-1)
            };

            // Act & Assert
            // Note: Make sure AppTaskManager.CreateTaskWithRelationsAsync throws ArgumentException("Geçmiş...") for this to pass.
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _taskManager.CreateTaskWithRelationsAsync(task, new List<int>(), 1));
                
            Assert.Contains("Geçmiş", exception.Message);
        }

        [Fact]
        public async Task CreateTaskWithRelationsAsync_SuccessfullyCallsAddAndSave_WhenDueDateIsValid()
        {
            // Arrange
            var task = new AppTask
            {
                Title = "Geçerli Görev",
                DueDate = DateTime.Now.AddDays(5) // Valid future date
            };

            _mockTaskRepo.Setup(r => r.AddAsync(It.IsAny<AppTask>())).Returns(Task.CompletedTask);
            _mockTaskRepo.Setup(r => r.SaveAsync()).ReturnsAsync(1);

            // Act
            var result = await _taskManager.CreateTaskWithRelationsAsync(task, new List<int>(), 1);

            // Assert
            Assert.True(result.Success);
            _mockTaskRepo.Verify(r => r.AddAsync(It.Is<AppTask>(t => t.Title == "Geçerli Görev")), Times.Once);
            _mockTaskRepo.Verify(r => r.SaveAsync(), Times.Once);
        }
    }
}
