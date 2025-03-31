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
            //TODO check user status to be active (in other methods as well)
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new EntityNotFoundException(ErrorMessages.UserNotAuthenticated);
            }

            user.Balance = user.Balance.Add(request.Amount);
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }
    }
}
