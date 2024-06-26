﻿

using Employee.Infrastructure.Persistence.Repository.UserRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Interfaces.ITaskRepository;
using TaskManager.Domain.Interfaces.IUserRepository;
using TaskManager.Domain.Services;
using TaskManager.Infrastructure.Persistence.EFCore;
using TaskManager.Infrastructure.Persistence.Repository.TaskRepository;

namespace TaskManager.API.Configuration
{
    public static class TaskManagerConfiguration
    {
        public static async void TaskManagerServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddDbContext<TaskManagerDbContext>(
                options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }
           );

            // For Identity  
            services.AddIdentity<UserDetail, IdentityRole>()
                            .AddEntityFrameworkStores<TaskManagerDbContext>()
                            .AddDefaultTokenProviders();


            // Adding Authentication  
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                 // Adding Jwt Bearer  
                 .AddJwtBearer(options =>
                  {
                      options.SaveToken = true;
                      options.RequireHttpsMetadata = false;
                      options.TokenValidationParameters = new TokenValidationParameters()
                      {
                          ValidateIssuer = true,
                          ValidateAudience = true,
                          ValidAudience = configuration["JWT:ValidAudience"],
                          ValidIssuer = configuration["JWT:ValidIssuer"],
                          ClockSkew = TimeSpan.Zero,
                          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                      };
                  });

            //application Serrvice
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtTokenManager, JwtTokenManager>();
            services.AddScoped<ITaskService, TaskService>();
            //application repository
            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();
            services.AddScoped<ITaskReadRepository, TaskReadRepository>();
            services.AddScoped<ITaskWriteRepository, TaskWriteRepository>();
        }

    }
}
