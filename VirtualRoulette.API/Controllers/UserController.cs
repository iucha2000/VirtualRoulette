using MediatR;
using Microsoft.AspNetCore.Authorization;
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
            var query = new GetBalanceQuery(HttpContext.GetUserId());

            var result = await _mediator.Send(query);
            return Ok(ResponseWrapperDto<UserBalanceDto>.Success(result));
        }

        [Authorize]
        [HttpPost("deposit-funds")]
        public async Task<IActionResult> DepositFunds([FromBody] DepositFundsDto depositFundsDto)
        {
            var command = new DepositFundsCommand(HttpContext.GetUserId(), depositFundsDto.Amount);

            await _mediator.Send(command);
            return Ok(ResponseWrapperDto.Success(ClientMessages.DepositFundsSuccess));
        }

        [Authorize]
        [HttpPost("withdraw-funds")]
        public async Task<IActionResult> WithdrawFunds([FromBody] WithdrawFundsDto withdrawFundsDto)
        {
            var command = new WithdrawFundsCommand(HttpContext.GetUserId(), withdrawFundsDto.Amount);

            await _mediator.Send(command);
            return Ok(ResponseWrapperDto.Success(ClientMessages.DepositFundsSuccess));
        }

        [Authorize]
        [HttpGet("history")]
        public async Task<IActionResult> GameHistory([FromQuery] GameHistoryRequestDto requestDto)
        {
            var query = new GetGameHistoryQuery(HttpContext.GetUserId(), requestDto.PageIndex, requestDto.PageSize);

            var result = await _mediator.Send(query);
            return Ok(ResponseWrapperDto<GameHistoryResponseDto>.Success(result));
        }
    }
}
