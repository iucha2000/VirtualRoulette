using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.Interfaces.Repositories;
using VirtualRoulette.Application.Interfaces.Services;
using VirtualRoulette.Shared.Constants;

namespace VirtualRoulette.Infrastructure.Services
{
    public class InactiveUserSignOutService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(NumberValues.InactivityCheckPeriod);

        public InactiveUserSignOutService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using(var scope = _serviceProvider.CreateScope())
                {
                    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                    var jackpotHubService = scope.ServiceProvider.GetRequiredService<IJackpotHubService>();

                    var inactiveUsers = await userRepository.GetInactiveUsersAsync();

                    foreach (var user in inactiveUsers)
                    {
                        user.SignOut();
                        userRepository.Update(user);

                        await jackpotHubService.DisconnectUser(user.Id);
                    }
                    await userRepository.SaveChangesAsync();
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}
