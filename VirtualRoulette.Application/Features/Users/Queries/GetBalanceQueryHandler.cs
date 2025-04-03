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
    public class GetBalanceQueryHandler : IRequestHandler<GetBalanceQuery, UserBalanceDto>
    {
        private readonly IUserRepository _userRepository;

        public GetBalanceQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserBalanceDto> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
        {
            //Check if user is authenticated and is active
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if(user == null || !user.IsActive)
            {
                throw new EntityNotFoundException(ErrorMessages.UserNotAuthenticated);
            }

            //Update user activity
            user.UpdateLastActivity();
            await _userRepository.SaveChangesAsync();

            return new UserBalanceDto { Amount = user.Balance.CentAmount };
        }
    }
}
