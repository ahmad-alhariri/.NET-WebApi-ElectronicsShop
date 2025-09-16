using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicsShop.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IBulkProductService, BulkProductService>();

        return services;
    }
}