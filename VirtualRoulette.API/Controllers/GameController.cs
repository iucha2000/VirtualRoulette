using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualRoulette.Application.DTOs;
using VirtualRoulette.Application.Features.Bets.Commands;
using VirtualRoulette.Application.Interfaces.Repositories;
using VirtualRoulette.Application.Interfaces.Services;
using VirtualRoulette.Domain.Entities;
using VirtualRoulette.Shared.Extensions;

namespace VirtualRoulette.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IJackpotRepository _jackpotRepository;
        private readonly IJackpotHubService _jackpotHubService;

        public GameController(IMediator mediator, IJackpotRepository jackpotRepository, IJackpotHubService jackpotHubService)
        {
            _mediator = mediator;
            _jackpotRepository = jackpotRepository;
            _jackpotHubService = jackpotHubService;
        }

        [Authorize]
        [HttpPost("bet")]
        public async Task<IActionResult> MakeBet([FromBody] MakeBetRequestDto makeBetDto)
        {
            var command = new MakeBetCommand { UserId = HttpContext.GetUserId(), Bet = makeBetDto.Bet, UserIP = HttpContext.GetUserIpAddress(), CreatedAt = DateTime.UtcNow };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("jackpot")]
        public async Task<IActionResult> CurrentJackpot()
        {
            //TODO implement it
            return Ok();
        }

        [HttpPost("update-jackpot")]
        public async Task<IActionResult> UpdateJackpot(long newAmount)
        {
            var jackpot = new Jackpot();
            jackpot.AddToJackpot(newAmount);
            await _jackpotRepository.AddAsync(jackpot);
            await _jackpotRepository.SaveChangesAsync();

            await _jackpotHubService.PushJackpotUpdate(newAmount);
            return Ok();
        }
    }
}
