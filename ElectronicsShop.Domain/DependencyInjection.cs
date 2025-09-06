using ElectronicsShop.Domain.Common;
using ElectronicsShop.Domain.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicsShop.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        return services;
    }
}