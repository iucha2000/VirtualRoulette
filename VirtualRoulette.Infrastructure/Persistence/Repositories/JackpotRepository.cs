using Microsoft.EntityFrameworkCore;
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

namespace VirtualRoulette.Infrastructure.Persistence.Repositories
{
    public class JackpotRepository : Repository<Jackpot>, IJackpotRepository
    {
        private readonly IJackpotHubService _jackpotHubService;

        public JackpotRepository(VirtualRouletteDbContext dbContext, IJackpotHubService jackpotHubService) : base(dbContext)
        {
            _jackpotHubService = jackpotHubService;
        }

        public async Task<Jackpot?> GetLatestJackpotAsync()
        {
            return await _dbSet.OrderByDescending(j => j.UpdatedAt).FirstOrDefaultAsync();
        }

        public async Task IncreaseJackpotAmountAsync(long betAmount)
        {
            var currentJackpot = await GetLatestJackpotAsync();
            if (currentJackpot == null)
            {
                throw new EntityNotFoundException(ErrorMessages.JackpotNotFound);
            }

            currentJackpot.AddPercentageToJackpot(betAmount, NumberValues.JackpotIncreasePercentage);

            await SaveChangesAsync();

            await _jackpotHubService.PushJackpotUpdate(currentJackpot.Amount.CentAmount);
        }
    }
}
