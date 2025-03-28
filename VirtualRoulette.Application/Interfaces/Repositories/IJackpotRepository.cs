using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Domain.Entities;

namespace VirtualRoulette.Application.Interfaces.Repositories
{
    public interface IJackpotRepository : IRepository<Jackpot>
    {
        Task<Jackpot?> GetLatestJackpotAsync();
    }
}
