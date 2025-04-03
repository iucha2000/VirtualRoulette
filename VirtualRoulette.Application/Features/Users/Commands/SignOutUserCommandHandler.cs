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
            //Check if user is authenticated and is active
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null || !user.IsActive)
            {
                throw new EntityNotFoundException(ErrorMessages.UserNotAuthenticated);
            }

            //Change user status to inactive
            user.SignOut();
            await _userRepository.SaveChangesAsync();

            //Disconnect user from JackpotHub
            await _jackpotHubService.DisconnectUser(user.Id);
        }
    }
}
