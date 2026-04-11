using System.Threading.Tasks;
using BCrypt.Net;
using CrmProject.Business.Concrete;
using CrmProject.DataAccess.Abstract;
using CrmProject.Entity.Entities;
using Moq;
using Xunit;

namespace CrmProject.Tests.Security
{
    public class UserManagerTests
    {
        [Fact]
        public async Task AddUserAsync_SuccessfullyHashesPassword_BeforeCallingRepository()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepository>();
            var mockRoleRepo = new Mock<IRoleRepository>();
            var userManager = new UserManager(mockUserRepo.Object, mockRoleRepo.Object);

            var user = new User
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@user.com",
                PasswordHash = "plain_password"
            };

            mockUserRepo.Setup(repo => repo.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            mockUserRepo.Setup(repo => repo.SaveAsync()).ReturnsAsync(1);

            // Act
            var result = await userManager.AddUserAsync(user);

            // Assert
            Assert.True(result.Success);
            
            // Verify it was hashed before saving
            Assert.NotEqual("plain_password", user.PasswordHash);
            Assert.True(BCrypt.Net.BCrypt.Verify("plain_password", user.PasswordHash));
            
            mockUserRepo.Verify(repo => repo.AddAsync(It.Is<User>(u => u.PasswordHash == user.PasswordHash)), Times.Once);
            mockUserRepo.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}
