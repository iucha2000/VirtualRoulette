using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Application.Interfaces.Services
{
    public interface IJackpotHubService
    {
        Task ConnectUser(Guid userId);
        Task DisconnectUser(Guid userId);
        Task PushJackpotUpdate(decimal newJackpotAmount);
    }
}
