using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.DTOs;
using VirtualRoulette.Application.Interfaces.Repositories;
using VirtualRoulette.Domain.Exceptions;
using VirtualRoulette.Shared.Constants;

namespace VirtualRoulette.Application.Features.Users.Queries
{
    public class GetGameHistoryQueryHandler : IRequestHandler<GetGameHistoryQuery, GameHistoryResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBetRepository _betRepository;

        public GetGameHistoryQueryHandler(IUserRepository userRepository, IBetRepository betRepository)
        {
            _userRepository = userRepository;
            _betRepository = betRepository;
        }

        public async Task<GameHistoryResponseDto> Handle(GetGameHistoryQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null || !user.IsActive)
            {
                throw new EntityNotFoundException(ErrorMessages.UserNotAuthenticated);
            }

            var userBets = await _betRepository.GetBetsByUserIdAsync(user.Id, request.PageIndex, request.PageSize);

            var userGames = userBets.Select(b => new GameHistoryEntry
            {
                SpinId = b.SpinId,
                BetAmount = b.BetAmount.Amount,
                WonAmount = b.WonAmount.Amount,
                CreatedAt = b.CreatedAt,
            }).ToList();

            user.UpdateLastActivity();
            await _userRepository.SaveChangesAsync();

            return new GameHistoryResponseDto { Entries = userGames };
        }
    }
}
