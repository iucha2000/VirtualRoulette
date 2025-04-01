using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualRoulette.Application.DTOs;
using VirtualRoulette.Application.Features.Users.Commands;
using VirtualRoulette.Application.Features.Users.Queries;
using VirtualRoulette.Shared.Extensions;

namespace VirtualRoulette.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("balance")]
        public async Task<IActionResult> Balance()
        {
            var query = new GetBalanceQuery { UserId = HttpContext.GetUserId() };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("deposit-funds")]
        public async Task<IActionResult> DepositFunds([FromBody] DepositFundsDto depositFundsDto)
        {
            var command = new DepositFundsCommand { UserId = HttpContext.GetUserId(), Amount = depositFundsDto.Amount };

            await _mediator.Send(command);
            return Ok();
        }

        [Authorize]
        [HttpPost("withdraw-funds")]
        public async Task<IActionResult> WithdrawFunds([FromBody] WithdrawFundsDto withdrawFundsDto)
        {
            var command = new WithdrawFundsCommand { UserId = HttpContext.GetUserId(), Amount = withdrawFundsDto.Amount };

            await _mediator.Send(command);
            return Ok();
        }

        [Authorize]
        [HttpGet("history")]
        public async Task<IActionResult> GameHistory([FromQuery] GameHistoryRequestDto requestDto)
        {
            var query = new GetGameHistoryQuery { UserId = HttpContext.GetUserId(), PageIndex = requestDto.PageIndex, PageSize = requestDto.PageSize };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
