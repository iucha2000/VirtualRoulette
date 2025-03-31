
using Microsoft.EntityFrameworkCore;
using VirtualRoulette.API.Hubs;
using VirtualRoulette.API.Middlewares;
using VirtualRoulette.Application;
using VirtualRoulette.Application.Interfaces.Repositories;
using VirtualRoulette.Application.Interfaces.Services;
using VirtualRoulette.Infrastructure;
using VirtualRoulette.Infrastructure.Persistence;
using VirtualRoulette.Infrastructure.Persistence.Repositories;
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

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();

            app.UseCors(TextValues.AllowAllCors);
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseWebSockets();

            app.UseMiddleware<UserActivityMiddleware>();

            app.MapControllers();
            app.MapHub<JackpotHub>(TextValues.JackpotHubPath);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Run();
        }
    }
}
