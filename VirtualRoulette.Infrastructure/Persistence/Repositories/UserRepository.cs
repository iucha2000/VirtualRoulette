using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.Interfaces.Repositories;
using VirtualRoulette.Domain.Entities;
using VirtualRoulette.Shared.Constants;

namespace VirtualRoulette.Infrastructure.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(VirtualRouletteDbContext dbContext) : base(dbContext) { }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<List<User>> GetInactiveUsersAsync(int inactivityMinutes = NumberValues.InactivityPeriod)
        {
            var cutoffTime = DateTime.UtcNow.AddMinutes(-inactivityMinutes);

            return await _dbSet
                .Where(u => u.LastActivity < cutoffTime && u.IsActive)
                .ToListAsync();
        }
    }
}
