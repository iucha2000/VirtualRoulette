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
    public class BetRepository : Repository<Bet>, IBetRepository
    {
        public BetRepository(VirtualRouletteDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Bet>> GetBetsByUserIdAsync(Guid userId)
        {
            return await _dbSet.Where(b => b.UserId == userId).ToListAsync();
        }
    }
}
