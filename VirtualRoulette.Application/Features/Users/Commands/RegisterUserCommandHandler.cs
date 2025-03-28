using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.Interfaces.Repositories;
using VirtualRoulette.Application.Interfaces.Services;
using VirtualRoulette.Domain.Entities;
using VirtualRoulette.Domain.Exceptions;
using VirtualRoulette.Shared.Constants;

namespace VirtualRoulette.Application.Features.Users.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IJwtTokenService _jwtTokenService;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHashService passwordHashService, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<string?> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);

            if (existingUser != null)
            {
                throw new DuplicateEntityException(ErrorMessages.UserAlreadyExists);
            }

            var hashedPassword = _passwordHashService.HashPassword(request.Password);

            var user = new User(request.Username, hashedPassword);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return _jwtTokenService.GenerateToken(user);
        }
    }
}
