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

namespace VirtualRoulette.Application.Features.Jackpots.Queries
{
    public class GetCurrentJackpotQueryHandler : IRequestHandler<GetCurrentJackpotQuery, CurrentJackpotDto>
    {
        private readonly IJackpotRepository _jackpotRepository;
        private readonly IUserRepository _userRepository;

        public GetCurrentJackpotQueryHandler(IJackpotRepository jackpotRepository, IUserRepository userRepository)
        {
            _jackpotRepository = jackpotRepository;
            _userRepository = userRepository;
        }

        public async Task<CurrentJackpotDto> Handle(GetCurrentJackpotQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new EntityNotFoundException(ErrorMessages.UserNotAuthenticated);
            }

            var currentJackpot = await _jackpotRepository.GetLatestJackpotAsync();
            if (currentJackpot == null)
            {
                throw new EntityNotFoundException(ErrorMessages.JackpotNotFound);
            }

            return new CurrentJackpotDto { Amount = currentJackpot.Amount.Amount };
        }
    }
}
