using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string?> RegisterAsync(string username, string password);
        Task<string?> LoginAsync(string username, string password);
    }
}
