using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Domain.Enums
{
    public enum BetStatus
    {
        Pending,
        Accepted,
        Rejected,
        Won,
        Lost
    }
}
