using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Infrastructure.Persistence.Initialization
{
    public static class MigrationsInitializer
    {
        public static void ApplyMigrations<TContext>(this WebApplication app) where TContext : DbContext
        {
            using(var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetRequiredService<TContext>();

                logger.LogInformation("Checking for pending database migrations...");

                context.Database.Migrate();

                logger.LogInformation("Database is up to date");
            }
        }
    }
}
