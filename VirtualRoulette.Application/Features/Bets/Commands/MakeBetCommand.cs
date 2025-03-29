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
    public class MakeBetCommand : IRequest<MakeBetResponseDto>
    {
        public Guid UserId { get; set; }
        public string Bet {  get; set; } = string.Empty;
        public string UserIP { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
