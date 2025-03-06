using Azure.Core;
using ECommerce.Service.AuthAPI.Data;
using ECommerce.Service.AuthAPI.Dto.Requests;
using ECommerce.Service.AuthAPI.Dto.Response;
using ECommerce.Service.AuthAPI.Models;
using ECommerce.Service.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Service.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string Email, string RoleName)
        {
            var user = _context.ApplicationUsers.FirstOrDefault((user) => user.Email.ToLower() == Email.ToLower());

            if (user == null) return false;


            if (!_roleManager.RoleExistsAsync(RoleName).GetAwaiter().GetResult())
            {
                //Create Role If not Exist
                _roleManager.CreateAsync(new IdentityRole(RoleName)).GetAwaiter().GetResult();
            }

            await _userManager.AddToRoleAsync(user, RoleName);

            return true;

        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO request)
        {
            var loginResponse = new LoginResponseDTO();
            var user = _context.ApplicationUsers.FirstOrDefault((user) => user.Email.ToLower() == request.Email.ToLower());

            if (user == null)
            {
                loginResponse.User = null;
                loginResponse.Token = null;
                return loginResponse;
            }

            bool isValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isValid)
            {
                loginResponse.User = null;
                loginResponse.Token = null;
                return loginResponse;
            }

            var userResponse = new UserResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtTokenGenerator.GenerateToken(user,roles);

            loginResponse.User = userResponse;
            loginResponse.Token = token;


            return loginResponse;
        }

        public async Task<string> Register(RegisterRequestDTO request)
        {
            ApplicationUser user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                NormalizedEmail = request.Email.ToUpper(),
            };

            try
            {

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    var userToReturn = _context.ApplicationUsers.First((user) => user.Email == request.Email);

                    var userResponse = new UserResponseDTO
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber
                    };

                    return "Registeration Successfully";

                }
                else
                {
                    return "Registeration Failed: " + result.Errors.FirstOrDefault().Description;
                }

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}
