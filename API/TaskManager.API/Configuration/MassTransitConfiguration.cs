using MassTransit;
using TaskManager.API.Extensions;

namespace TaskManager.API.Configuration
{
    public static class MassTransitConfiguration
    {
            public static void AddMassTransitComponents(this IServiceCollection services, IConfiguration configuration)
            {
                services.AddMediator(x =>
                {   // commands
                  
                    x.ConfigureMediator((context, cfg) => cfg.UseHttpContextScopeFilter(context));
                });

            }
    }
}
