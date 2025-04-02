
using Serilog;
using VirtualRoulette.API.DependencyInjection;
using VirtualRoulette.API.Hubs;
using VirtualRoulette.API.Middlewares;
using VirtualRoulette.Application.DependencyInjection;
using VirtualRoulette.Infrastructure.DependencyInjection;
using VirtualRoulette.Infrastructure.Persistence;
using VirtualRoulette.Infrastructure.Persistence.Initialization;
using VirtualRoulette.Shared.Constants;

namespace VirtualRoulette.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddInfrastructure(builder.Configuration)
                .AddApplication()
                .AddApi();

            builder.Host.UseSerilog();

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();

            app.UseCors(TextValues.AllowAllCors);
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseWebSockets();

            app.MapControllers();
            app.MapHub<JackpotHub>(TextValues.JackpotHubPath);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.ApplyMigrations<VirtualRouletteDbContext>();

            app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            app.Run();
        }
    }
}
