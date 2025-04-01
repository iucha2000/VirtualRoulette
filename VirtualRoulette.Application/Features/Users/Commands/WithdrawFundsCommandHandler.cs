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
    public class WithdrawFundsCommandHandler : IRequestHandler<WithdrawFundsCommand>
    {
        private readonly IUserRepository _userRepository;

        public WithdrawFundsCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(WithdrawFundsCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null || !user.IsActive)
            {
                throw new EntityNotFoundException(ErrorMessages.UserNotAuthenticated);
            }

            if(user.Balance.Amount < request.Amount)
            {
                throw new NotEnoughBalanceException(ErrorMessages.NotEnoughBalance);
            }

            user.Balance = user.Balance.Subtract(request.Amount);
            user.UpdateLastActivity();
            await _userRepository.SaveChangesAsync();
        }
    }
}
