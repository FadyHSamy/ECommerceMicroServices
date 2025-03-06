using ECommerce.Service.AuthAPI.Models;

namespace ECommerce.Service.AuthAPI.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser user,IEnumerable<string> roles);
    }
}
