﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.DTOs;
using VirtualRoulette.Application.Interfaces.Repositories;
using VirtualRoulette.Application.Interfaces.Services;
using VirtualRoulette.Domain.Entities;
using VirtualRoulette.Domain.Exceptions;
using VirtualRoulette.Shared.Constants;

namespace VirtualRoulette.Application.Features.Users.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, TokenResponseDto>
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

        public async Task<TokenResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            //Check if user with given username already exists in the database
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null)
            {
                throw new DuplicateEntityException(ErrorMessages.UserAlreadyExists);
            }

            //Generate hash for user password
            var hashedPassword = _passwordHashService.HashPassword(request.Password);

            //Add new user to database and update activity
            var user = new User(request.Username, hashedPassword);
            user.UpdateLastActivity();

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return new TokenResponseDto { Token = _jwtTokenService.GenerateToken(user) };
        }
    }
}
