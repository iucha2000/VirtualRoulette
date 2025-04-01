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
    public class GetUserBalanceQueryHandler : IRequestHandler<GetUserBalanceQuery, UserBalanceDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserBalanceQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserBalanceDto> Handle(GetUserBalanceQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if(user == null)
            {
                throw new EntityNotFoundException(ErrorMessages.UserNotAuthenticated);
            }

            return new UserBalanceDto { Amount = user.Balance.Amount };
        }
    }
}
