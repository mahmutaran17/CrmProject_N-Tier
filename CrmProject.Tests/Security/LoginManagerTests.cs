using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrmProject.Business.Abstract;
using CrmProject.Business.Concrete;
using CrmProject.Entity.Entities;
using Moq;
using Xunit;

namespace CrmProject.Tests.Security
{
    public class LoginManagerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IRoleService> _mockRoleService;
        private readonly LoginManager _loginManager;

        public LoginManagerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockRoleService = new Mock<IRoleService>();
            _loginManager = new LoginManager(_mockUserService.Object, _mockRoleService.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsClaimsPrincipal_WhenPasswordMatchesBCryptHash()
        {
            // Arrange
            var email = "admin@crm.com";
            var password = "correct_password";
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            var activeUser = new User { Id = 1, Email = email, PasswordHash = hash, IsActive = true, RoleId = 1, FirstName = "Admin", LastName = "User" };

            _mockUserService.Setup(s => s.GetWhereAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { activeUser });
            _mockRoleService.Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(new Role { Id = 1, RoleName = "Admin" });

            // Act
            var principal = await _loginManager.AuthenticateAsync(email, password);

            // Assert
            Assert.NotNull(principal);
            Assert.True(principal.Identity!.IsAuthenticated);
            Assert.Contains(principal.Claims, c => c.Value == "Admin User");
        }

        [Fact]
        public async Task AuthenticateAsync_ReturnsNull_WhenBCryptVerificationFails()
        {
            // Arrange
            var email = "admin@crm.com";
            var correctPassword = "correct_password";
            var wrongPassword = "wrong_password";
            var hash = BCrypt.Net.BCrypt.HashPassword(correctPassword);
            var activeUser = new User { Id = 1, Email = email, PasswordHash = hash, IsActive = true, RoleId = 1 };

            _mockUserService.Setup(s => s.GetWhereAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { activeUser });

            // Act
            var principal = await _loginManager.AuthenticateAsync(email, wrongPassword);

            // Assert
            Assert.Null(principal);
        }
    }
}
