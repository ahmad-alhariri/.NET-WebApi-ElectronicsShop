

using ElectronicsShop.Application.Common.Settings;

namespace ElectronicsShop.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CurrencySettings>(configuration.GetSection("CurrencySettings"));

        return services;
    }

}