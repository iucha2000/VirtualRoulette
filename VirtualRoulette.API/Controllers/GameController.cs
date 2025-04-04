﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualRoulette.Application.DTOs;
using VirtualRoulette.Application.Features.Bets.Commands;
using VirtualRoulette.Application.Features.Jackpots.Queries;
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

        public GameController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("bet")]
        public async Task<IActionResult> MakeBet([FromBody] MakeBetRequestDto makeBetDto)
        {
            var command = new MakeBetCommand(HttpContext.GetUserId(), makeBetDto.Bet, HttpContext.GetUserIpAddress(), DateTime.UtcNow);
            
            var result = await _mediator.Send(command);
            return Ok(ResponseWrapperDto<MakeBetResponseDto>.Success(result));
        }

        [Authorize]
        [HttpGet("jackpot")]
        public async Task<IActionResult> CurrentJackpot()
        {
            var query = new GetCurrentJackpotQuery(HttpContext.GetUserId());

            var result = await _mediator.Send(query);
            return Ok(ResponseWrapperDto<CurrentJackpotDto>.Success(result));
        }
    }
}
