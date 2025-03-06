namespace ECommerce.Service.AuthAPI.Dto.Response
{
    public class LoginResponseDTO
    {
        public UserResponseDTO User { get; set; }
        public string Token { get; set; }
    }
}
