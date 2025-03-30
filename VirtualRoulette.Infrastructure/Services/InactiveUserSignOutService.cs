using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.Interfaces.Repositories;

namespace VirtualRoulette.Infrastructure.Services
{
    public class InactiveUserSignOutService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);

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

                    var inactiveUsers = await userRepository.GetInactiveUsersAsync();

                    foreach (var user in inactiveUsers)
                    {
                        //TODO think about managing JWT token when signed out
                        user.SignOut();
                        userRepository.Update(user);
                    }
                    await userRepository.SaveChangesAsync();
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}
