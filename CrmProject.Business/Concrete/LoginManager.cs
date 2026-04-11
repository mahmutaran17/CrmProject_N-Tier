using System.Security.Claims;
using CrmProject.Business.Abstract;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CrmProject.Business.Concrete
{
    public class LoginManager : ILoginService
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public LoginManager(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public async Task<ClaimsPrincipal?> AuthenticateAsync(string email, string password)
        {
            // GAP-B4: Fetch by email + active only. Password verified via BCrypt below.
            var users = await _userService.GetWhereAsync(x => x.Email == email && x.IsActive);
            var user = users.FirstOrDefault();

            if (user == null) return null;

            // BCrypt hash verification — constant-time comparison, safe against timing attacks
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

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
            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}
