using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.Interfaces.Repositories;
using VirtualRoulette.Domain.Entities;

namespace VirtualRoulette.Infrastructure.Persistence.Repositories
{
    public class JackpotRepository : Repository<Jackpot>, IJackpotRepository
    {
        public JackpotRepository(VirtualRouletteDbContext dbContext) : base(dbContext) { }

        public async Task<Jackpot?> GetLatestJackpotAsync()
        {
            return await _dbSet.OrderByDescending(j => j.UpdatedAt).FirstOrDefaultAsync();
        }
    }
}
