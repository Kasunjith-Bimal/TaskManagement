using MassTransit;
using TaskManager.API.Extensions;
using TaskManager.Application.Command.AuthenticationReleted.ChangePassword;
using TaskManager.Application.Command.AuthenticationReleted.Login;
using TaskManager.Application.Command.AuthenticationReleted.Register;

namespace TaskManager.API.Configuration
{
    public static class MassTransitConfiguration
    {
            public static void AddMassTransitComponents(this IServiceCollection services, IConfiguration configuration)
            {
                services.AddMediator(x =>
                {   // commands
                    x.AddConsumer<ChangePassword>();
                    x.AddConsumer<LoginUser>();
                    x.AddConsumer<RegisterUser>();

                    x.ConfigureMediator((context, cfg) => cfg.UseHttpContextScopeFilter(context));
                });

            }
    }
}
