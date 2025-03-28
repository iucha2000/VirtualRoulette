using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualRoulette.Application.DTOs;
using VirtualRoulette.Application.Features.Users.Commands;
using VirtualRoulette.Application.Features.Users.Queries;

namespace VirtualRoulette.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            var token = await _mediator.Send(new RegisterUserCommand { Username = registerUserDto.Username, Password = registerUserDto.Password });
            return token == null ? BadRequest("Registration failed") : Ok(new { Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var token = await _mediator.Send(new LoginUserQuery { Username = loginUserDto.Username, Password = loginUserDto.Password });
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
