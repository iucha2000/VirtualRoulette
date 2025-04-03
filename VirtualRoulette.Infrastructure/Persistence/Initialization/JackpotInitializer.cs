using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualRoulette.Application.Interfaces.Repositories;
using VirtualRoulette.Domain.Entities;
using VirtualRoulette.Shared.Constants;

namespace VirtualRoulette.Infrastructure.Persistence.Initialization
{
    //Initialize new jackpot at application startup if current jackpot does not exist
    public class JackpotInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public JackpotInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var jackpotRepository = scope.ServiceProvider.GetRequiredService<IJackpotRepository>();

                var currentJackpot = await jackpotRepository.GetLatestJackpotAsync();
                if (currentJackpot == null)
                {
                    var jackpot = new Jackpot();

                    await jackpotRepository.AddAsync(jackpot);
                    await jackpotRepository.SaveChangesAsync();
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
