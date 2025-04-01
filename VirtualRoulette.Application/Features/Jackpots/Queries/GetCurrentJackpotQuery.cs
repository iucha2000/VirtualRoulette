using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.DTOs;

namespace VirtualRoulette.Application.Features.Jackpots.Queries
{
    public class GetCurrentJackpotQuery : IRequest<CurrentJackpotDto>
    {
        public Guid UserId { get; set; }
    }
}
