using Microsoft.Extensions.Configuration;
using VirtualRoulette.Application.Interfaces.Repositories;
using VirtualRoulette.Application.Interfaces.Services;
using VirtualRoulette.Domain.Entities;

namespace VirtualRoulette.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(IUserRepository userRepository, IPasswordHashService passwordHashService, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _passwordHashService = passwordHashService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<string?> RegisterAsync(string username, string password)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(username);
            if(existingUser != null)
            {
                //TODO throw exception when user already exists
                return null;
            }

            var hashedPassword = _passwordHashService.HashPassword(password);
            var user = new User(username, hashedPassword);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return _jwtTokenService.GenerateToken(user);
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || !_passwordHashService.VerifyPassword(user.PasswordHash, password))
            {
                //TODO throw exception if necessary
                return null;
            }

            return _jwtTokenService.GenerateToken(user);
        }
    }
}
