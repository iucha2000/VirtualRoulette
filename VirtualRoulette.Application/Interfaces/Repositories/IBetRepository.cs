using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Domain.Entities;

namespace VirtualRoulette.Application.Interfaces.Repositories
{
    public interface IBetRepository : IRepository<Bet>
    {
        Task<IEnumerable<Bet>> GetBetsByUserIdAsync(Guid userId, int pageIndex, int pageSize);
    }
}
