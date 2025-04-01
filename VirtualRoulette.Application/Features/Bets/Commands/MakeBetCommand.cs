using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.DTOs;
using VirtualRoulette.Domain.ValueObjects;

namespace VirtualRoulette.Application.Features.Bets.Commands
{
    public record MakeBetCommand(Guid UserId, string Bet, string UserIP, DateTime CreatedAt) : IRequest<MakeBetResponseDto>;
}
