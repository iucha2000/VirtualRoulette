using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualRoulette.Application.DTOs;
using VirtualRoulette.Application.Features.Bets.Commands;
using VirtualRoulette.Shared.Extensions;

namespace VirtualRoulette.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GameController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("bet")]
        public async Task<IActionResult> MakeBet([FromBody] MakeBetRequestDto makeBetDto)
        {
            var command = new MakeBetCommand { UserId = HttpContext.GetUserId(), Bet = makeBetDto.Bet, UserIP = HttpContext.GetUserIpAddress(), CreatedAt = DateTime.UtcNow };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
