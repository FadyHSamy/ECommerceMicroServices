using ECommerce.Service.AuthAPI.Dto.Requests;
using ECommerce.Service.AuthAPI.Dto.Response;

namespace ECommerce.Service.AuthAPI.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Register(RegisterRequestDTO request);
        Task<LoginResponseDTO> Login(LoginRequestDTO request);
        Task<bool> AssignRole(string Email, string RoleName);
    }
}
