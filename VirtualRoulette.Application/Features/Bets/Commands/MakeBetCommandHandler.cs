using MediatR;
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
using VirtualRoulette.Domain.ValueObjects;

namespace VirtualRoulette.Application.Features.Bets.Commands
{
    public class MakeBetCommandHandler : IRequestHandler<MakeBetCommand, MakeBetResponseDto>
    {
        private readonly IBetRepository _betRepository;
        private readonly IBetAnalyzerService _betAnalyzerService;
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        public MakeBetCommandHandler(IBetRepository betRepository, IBetAnalyzerService betAnalyzerService)
        {
            _betRepository = betRepository;
            _betAnalyzerService = betAnalyzerService;
        }

        public async Task<MakeBetResponseDto> Handle(MakeBetCommand request, CancellationToken cancellationToken)
        {
            //TODO implement user balance changes
            //TODO implement jackpot changes

            //Check if bet has valid status
            if (!_betAnalyzerService.IsBetValid(request.Bet))
            {
                return new MakeBetResponseDto
                {
                    Status = Domain.Enums.BetStatus.Rejected,
                    SpinId = null,
                    WinningNumber = null,
                    WonAmount = null,
                };
            }

            //Add bet with initial data to the database
            var spinId = Guid.NewGuid();

            var bet = new Bet(request.UserId, spinId, request.Bet, request.UserIP);
            bet.BetAmount = new Money(_betAnalyzerService.GetBetAmount(request.Bet));
            bet.MarkAsAccepted();
            await _betRepository.AddAsync(bet);
            await _betRepository.SaveChangesAsync();

            //Generate winning number and check user win
            int winnum = GenerateSecureRandomNumber(0, 36);
            int wonAmount = _betAnalyzerService.EstimateBetWin(request.Bet, winnum);

            bet.WinningNumber = winnum;
            bet.UpdateWinnings(wonAmount);
            _betRepository.Update(bet);
            await _betRepository.SaveChangesAsync();

            //Return spin info to user
            return new MakeBetResponseDto
            {
                Status = bet.Status,
                SpinId = spinId,
                WinningNumber = winnum,
                WonAmount = wonAmount,
            };
        }

        private static int GenerateSecureRandomNumber(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), "Min cannot be greater than max");
            }
                
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
