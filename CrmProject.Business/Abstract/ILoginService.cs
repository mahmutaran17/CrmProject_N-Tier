using System.Security.Claims;

namespace CrmProject.Business.Abstract
{
    public interface ILoginService
    {
        Task<ClaimsPrincipal?> AuthenticateAsync(string email, string password);
    }
}
