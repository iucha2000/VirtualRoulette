using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Domain.Entities;
using VirtualRoulette.Shared.Constants;

namespace VirtualRoulette.Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<List<User>> GetInactiveUsersAsync(int inactivityMinutes = NumberValues.InactivityPeriod);
    }
}
