using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.Interfaces.Repositories;
using VirtualRoulette.Domain.Exceptions;
using VirtualRoulette.Shared.Constants;

namespace VirtualRoulette.Application.Features.Users.Commands
{
    public class DepositFundsCommandHandler : IRequestHandler<DepositFundsCommand>
    {
        private readonly IUserRepository _userRepository;

        public DepositFundsCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(DepositFundsCommand request, CancellationToken cancellationToken)
        {
            //Check if user is authenticated and is active
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null || !user.IsActive)
            {
                throw new EntityNotFoundException(ErrorMessages.UserNotAuthenticated);
            }

            //Add requested amount to balance
            user.Balance = user.Balance.Add(request.Amount);

            //Update user activity
            user.UpdateLastActivity();
            await _userRepository.SaveChangesAsync();
        }
    }
}
