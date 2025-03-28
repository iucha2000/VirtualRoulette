using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualRoulette.Application.DTOs.User;
using VirtualRoulette.Application.Interfaces.Services;

namespace VirtualRoulette.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRequest request)
        {
            var token = await _authService.RegisterAsync(request.Username, request.Password);
            return token == null ? BadRequest("Registration failed") : Ok(new { Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            var token = await _authService.LoginAsync(request.Username, request.Password);
            return token == null ? Unauthorized() : Ok(new { Token = token });
        }


        [Authorize]
        [HttpPost("something")]
        public async Task<IActionResult> Something(int n)
        {
            return Ok(n);
        }
    }
}
