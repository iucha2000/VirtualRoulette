using VirtualRoulette.Application.Interfaces.Repositories;
using VirtualRoulette.Infrastructure.Persistence.Repositories;
using VirtualRoulette.Shared.Extensions;

namespace VirtualRoulette.API.Middlewares
{
    public class UserActivityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public UserActivityMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.GetUserId();
            if (userId != Guid.Empty)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                    var user = await userRepository.GetByIdAsync(userId);
                    if (user != null)
                    {
                        user.UpdateLastActivity();
                        userRepository.Update(user);
                        await userRepository.SaveChangesAsync();
                    }
                }
            }

            await _next(context);
        }
    }
}
