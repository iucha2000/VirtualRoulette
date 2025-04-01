using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualRoulette.Application.DTOs;
using VirtualRoulette.Application.Features.Users.Commands;
using VirtualRoulette.Application.Features.Users.Queries;
using VirtualRoulette.Shared.Constants;
using VirtualRoulette.Shared.Extensions;

namespace VirtualRoulette.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            var command = new RegisterUserCommand { Username = registerUserDto.Username, Password = registerUserDto.Password };

            var token = await _mediator.Send(command);
            return token == null ? BadRequest(ErrorMessages.RegistrationFailed) : Ok(new { Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var query = new LoginUserQuery { Username = loginUserDto.Username, Password = loginUserDto.Password };

            var token = await _mediator.Send(query);
            return token == null ? BadRequest(ErrorMessages.LoginFailed) : Ok(new { Token = token });
        }

        [Authorize]
        [HttpPost("sign-out")]
        public async Task<IActionResult> Signout()
        {
            var command = new SignOutUserCommand { UserId = HttpContext.GetUserId() };

            await _mediator.Send(command);
            return Ok();
        }
    }
}
