using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.DTOs;

namespace VirtualRoulette.Application.Features.Users.Queries
{
    public class GetGameHistoryQuery : IRequest<GetGameHistoryResponseDto>
    {
        public Guid UserId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
