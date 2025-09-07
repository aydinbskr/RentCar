using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RentCarServer.Application.Behaviours;
using RentCarServer.Application.Services;
using TS.MediatR;

namespace RentCarServer.Application
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<PermissionService>();
            services.AddScoped<PermissionCleanerSevice>();
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(ServiceRegister).Assembly);
                config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
                config.AddOpenBehavior(typeof(PermissionBehaviour<,>));
            });
            services.AddValidatorsFromAssembly(typeof(ServiceRegister).Assembly);
            return services;
        }
    }
}
