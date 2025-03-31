using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.Interfaces.Repositories;
using VirtualRoulette.Application.Interfaces.Services;
using VirtualRoulette.Domain.Exceptions;
using VirtualRoulette.Shared.Constants;

namespace VirtualRoulette.Application.Features.Users.Commands
{
    public class SignOutUserCommandHandler : IRequestHandler<SignOutUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJackpotHubService _jackpotHubService;

        public SignOutUserCommandHandler(IUserRepository userRepository, IJackpotHubService jackpotHubService)
        {
            _userRepository = userRepository;
            _jackpotHubService = jackpotHubService;
        }

        public async Task Handle(SignOutUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new EntityNotFoundException(ErrorMessages.UserNotAuthenticated);
            }

            user.SignOut();
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            await _jackpotHubService.DisconnectUser(user.Id);
        }
    }
}
