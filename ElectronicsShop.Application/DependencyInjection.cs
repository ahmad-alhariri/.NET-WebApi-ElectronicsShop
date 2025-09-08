using System.Reflection;
using ElectronicsShop.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicsShop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //Configuration Of Automapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        // Get Validators
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            // cfg.AddOpenBehavior(typeof(PerformanceBehaviour<,>));
            // cfg.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));
            // cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
        });

        return services;
    }
}