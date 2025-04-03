using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.DTOs;
using VirtualRoulette.Application.Interfaces.Repositories;
using VirtualRoulette.Application.Interfaces.Services;
using VirtualRoulette.Domain.Exceptions;
using VirtualRoulette.Shared.Constants;

namespace VirtualRoulette.Application.Features.Users.Queries
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, TokenResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginUserQueryHandler(IUserRepository userRepository, IPasswordHashService passwordHashService, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<TokenResponseDto> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            //Check if user with given username exists in the database 
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null || !_passwordHashService.VerifyPassword(user.PasswordHash, request.Password))
            {
                throw new EntityNotFoundException(ErrorMessages.UserNotFound);
            }

            //Update user activity
            user.UpdateLastActivity();
            await _userRepository.SaveChangesAsync();

            return new TokenResponseDto { Token = _jwtTokenService.GenerateToken(user) };
        }
    }
}
