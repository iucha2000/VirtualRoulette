using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.Interfaces.Repositories;
using VirtualRoulette.Application.Interfaces.Services;

namespace VirtualRoulette.Application.Features.Users.Queries
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string?>
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

        public async Task<string?> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null || !_passwordHashService.VerifyPassword(user.PasswordHash, request.Password))
            {
                //TODO throw exception if user does not exist (if necessary)
                return null;
            }

            return _jwtTokenService.GenerateToken(user);
        }
    }
}
