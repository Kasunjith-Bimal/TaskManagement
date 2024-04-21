using MassTransit;
using TaskManager.API.Extensions;
using TaskManager.Application.Command.AuthenticationReleted.ChangePassword;
using TaskManager.Application.Command.AuthenticationReleted.Login;
using TaskManager.Application.Command.AuthenticationReleted.Register;
using TaskManager.Application.Command.TaskReleted.CreateTask;
using TaskManager.Application.Command.TaskReleted.DeleteTask;
using TaskManager.Application.Command.TaskReleted.UpdateTask;
using TaskManager.Application.Queries.TaskReleted.GetAllTaskByUserId;
using TaskManager.Application.Queries.TaskReleted.GetTaskById;
using TaskManager.Application.Queries.UserReleted.GetAllUsers;
using TaskManager.Application.Queries.UserReleted.GetUserById;

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
                    x.AddConsumer<CreateTask>();
                    x.AddConsumer<DeleteTask>();
                    x.AddConsumer<UpdateTask>();
                    //queries 

                    x.AddConsumer<GetUserById>();
                    x.AddConsumer<GetAllUsers>();
                    x.AddConsumer<GetAllTaskByUserId>();
                    x.AddConsumer<GetTaskById>();
                    x.ConfigureMediator((context, cfg) => cfg.UseHttpContextScopeFilter(context));
                });

            }
    }
}
