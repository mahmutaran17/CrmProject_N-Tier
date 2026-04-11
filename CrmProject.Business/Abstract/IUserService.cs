using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.Business.Abstract
{
    public interface IUserService : IGenericService<User>
    {
        Task<List<User>> GetActiveUsersAsync();
        Task<SelectList> GetRolesForDropdownAsync(int? selectedRoleId = null);
        Task<(bool Success, string Message)> AddUserAsync(User user);
        Task<(bool Success, string Message)> UpdateUserAsync(User user, IFormFile? profileImage);
        Task<(bool Success, string Message)> SoftDeleteUserAsync(int id);
        Task<User?> GetMyProfileAsync(int userId);
    }
}
