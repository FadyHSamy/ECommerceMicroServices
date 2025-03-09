using Azure;
using ECommerce.Service.AuthAPI.Dto;
using ECommerce.Service.AuthAPI.Dto.Requests;
using ECommerce.Service.AuthAPI.RabbitMQSender;
using ECommerce.Service.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Service.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRabbitMqAuthMessageSender _messageSender;
        private ResponseDto _response;
        public AuthAPIController(IAuthService authService, IRabbitMqAuthMessageSender messageSender)
        {
            _authService = authService;
            _response = new ResponseDto();
            _messageSender = messageSender;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            var errorMessage = await _authService.Register(request);

            if (string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }

            await _messageSender.SendMessageAsync(request.Email, "RegisterUserQueue");

            return Ok(_response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var result = await _authService.Login(request);

            if (result.Token != null)
            {
                _response.Result = result;
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid credentials";
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegisterRequestDTO request)
        {
            var isAssignerd = await _authService.AssignRole(request.Email, request.RoleName.ToUpper());

            if (isAssignerd)
            {
                _response.Message = "User Assigned Successfully";
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Error While Assigning";
            }

            return isAssignerd ? Ok(_response) : BadRequest(_response);
        }
    }
}
