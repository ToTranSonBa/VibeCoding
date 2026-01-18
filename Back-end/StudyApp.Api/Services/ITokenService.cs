using StudyApp.Api.Models;

namespace StudyApp.Api.Services
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user, IEnumerable<string>? roles = null);
        int GetExpiryMinutes();
    }
}
