using CrmProject.Business.Abstract;
using CrmProject.DataAccess.Abstract;
using CrmProject.Entity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmProject.Business.Concrete
{
    public class UserManager : GenericManager<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserManager(IUserRepository userRepository, IRoleRepository roleRepository) : base(userRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<List<User>> GetActiveUsersAsync()
        {
            return await _userRepository.GetWhereAsync(x => x.IsActive == true);
        }

        public async Task<SelectList> GetRolesForDropdownAsync(int? selectedRoleId = null)
        {
            var roles = await _roleRepository.GetAllAsync();
            return new SelectList(roles, "Id", "RoleName", selectedRoleId);
        }

        public async Task<(bool Success, string Message)> AddUserAsync(User user)
        {
            user.IsActive = true;
            user.RegistrationDate = DateTime.Now;

            // GAP-B4: Hash the password before persistence
            if (!string.IsNullOrWhiteSpace(user.PasswordHash))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveAsync();
            return (true, "Kullanici Başarıyla Eklendi.");
        }

        public async Task<(bool Success, string Message)> UpdateUserAsync(User user, IFormFile? profileImage)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
                return (false, "Güncellenecek kullanıcı bulunamadı.");

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.RoleId = user.RoleId;
            existingUser.IsActive = user.IsActive;

            // GAP-B4: Hash the new password if provided
            if (!string.IsNullOrWhiteSpace(user.PasswordHash))
                existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            if (profileImage != null && profileImage.Length > 0)
            {
                var extension = Path.GetExtension(profileImage.FileName);
                var newImageName = Guid.NewGuid().ToString() + extension;
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var saveLocation = Path.Combine(folderPath, newImageName);
                using (var stream = new FileStream(saveLocation, FileMode.Create))
                {
                    await profileImage.CopyToAsync(stream);
                }

                existingUser.ProfileImageUrl = "/images/profiles/" + newImageName;
            }

            _userRepository.Update(existingUser);
            await _userRepository.SaveAsync();
            return (true, "Profil başarıyla güncellendi.");
        }

        public async Task<(bool Success, string Message)> SoftDeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return (false, "Silinmek istenen personel bulunamadı!");

            user.IsActive = false;
            _userRepository.Update(user);
            await _userRepository.SaveAsync();
            return (true, $"{user.FirstName} {user.LastName} isimli personel sistemden silindi.");
        }

        public async Task<User?> GetMyProfileAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }
    }
}
