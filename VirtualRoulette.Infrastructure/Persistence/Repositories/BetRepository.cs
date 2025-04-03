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
    //BetRepository to implement all bet entity operation
    public class BetRepository : Repository<Bet>, IBetRepository
    {
        public BetRepository(VirtualRouletteDbContext dbContext) : base(dbContext) { }

        //Get bet list based on userId and pagionation parameters, ordered by creation date
        public async Task<IEnumerable<Bet>> GetBetsByUserIdAsync(Guid userId, int pageIndex, int pageSize)
        {
            return await _dbSet
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
