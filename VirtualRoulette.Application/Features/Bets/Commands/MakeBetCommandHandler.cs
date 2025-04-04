﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.DTOs;
using VirtualRoulette.Application.Interfaces.Repositories;
using VirtualRoulette.Application.Interfaces.Services;
using VirtualRoulette.Domain.Entities;
using VirtualRoulette.Domain.Enums;
using VirtualRoulette.Domain.Exceptions;
using VirtualRoulette.Domain.ValueObjects;
using VirtualRoulette.Shared.Constants;

namespace VirtualRoulette.Application.Features.Bets.Commands
{
    public class MakeBetCommandHandler : IRequestHandler<MakeBetCommand, MakeBetResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBetRepository _betRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJackpotRepository _jackpotRepository;
        private readonly IBetAnalyzerService _betAnalyzerService;

        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        public MakeBetCommandHandler(IUnitOfWork unitOfWork, IBetRepository betRepository, IUserRepository userRepository, IJackpotRepository jackpotRepository, IBetAnalyzerService betAnalyzerService)
        {
            _unitOfWork = unitOfWork;
            _betRepository = betRepository;
            _userRepository = userRepository;
            _jackpotRepository = jackpotRepository;
            _betAnalyzerService = betAnalyzerService;
        }

        public async Task<MakeBetResponseDto> Handle(MakeBetCommand request, CancellationToken cancellationToken)
        {
            //Check if bet has valid status
            if (!_betAnalyzerService.IsBetValid(request.Bet))
            {
                return new MakeBetResponseDto
                {
                    Status = BetStatus.Rejected,
                    SpinId = null,
                    WinningNumber = null,
                    WonAmount = null,
                    Message = ClientMessages.BetIsNotValid,
                };
            }

            //Check if user is authenticated and is active
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if(user == null || !user.IsActive)
            {
                throw new EntityNotFoundException(ErrorMessages.UserNotAuthenticated);
            }
            user.UpdateLastActivity();

            //Check if user has enough balance for provided bet amount
            var betAmount = new Money(_betAnalyzerService.GetBetAmount(request.Bet));
            if (user.Balance.CentAmount < betAmount.CentAmount)
            {
                return new MakeBetResponseDto
                {
                    Status = BetStatus.Rejected,
                    SpinId = null,
                    WinningNumber = null,
                    WonAmount = null,
                    Message = ClientMessages.UserHasNotEnoughBalance,
                };
            }
            user.Balance = user.Balance.Subtract(betAmount.CentAmount);

            //Update current jackpot amount and notify connected clients
            await _jackpotRepository.IncreaseJackpotAmountAsync(betAmount.CentAmount);

            //Add bet with initial data to the database with accepted status
            var spinId = Guid.NewGuid();
            var bet = new Bet(user.Id, spinId, request.Bet, betAmount, request.UserIP, request.CreatedAt);
            await _betRepository.AddAsync(bet);

            //Initially save accepted bet and user balance change to the database
            await _unitOfWork.SaveChangesAsync();
            
            //Generate winning number and check user won amount
            int winnum = GenerateSecureRandomNumber(NumberValues.RouletteMinValue, NumberValues.RouletteMaxValue);
            long wonAmount = _betAnalyzerService.EstimateBetWin(request.Bet, winnum);

            //Update user bet information with winning number and won amount
            bet.UpdateWinnings(winnum, wonAmount);
            if (wonAmount > 0)
            {
                user.Balance = user.Balance.Add(wonAmount);
            }

            //Finally save bet result and user balance change (if user won) to the database
            await _unitOfWork.SaveChangesAsync();

            //Return spin info to user
            return new MakeBetResponseDto
            {
                Status = bet.Status,
                SpinId = spinId,
                WinningNumber = winnum,
                WonAmount = wonAmount,
                Message = wonAmount > 0 ? ClientMessages.YouWonText : ClientMessages.YouLostText,
            };
        }

        //Method to generate secure radnom number for roulette
        private static int GenerateSecureRandomNumber(int min, int max)
        {
            uint range = (uint)(max - min + 1);
            uint limit = uint.MaxValue - (uint.MaxValue % range);

            byte[] randomBytes = new byte[4];
            uint result;

            do
            {
                _rng.GetBytes(randomBytes);
                result = BitConverter.ToUInt32(randomBytes, 0);
            } while (result >= limit);

            return (int)(result % range + min);
        }
    }   
}
