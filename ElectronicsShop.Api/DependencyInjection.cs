using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ElectronicsShop.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        
        services.AddHttpContextAccessor();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        // services.AddTransient<IUrlHelper>(x =>
        // {
        //     var factory = x.GetRequiredService<IUrlHelperFactory>();
        //     var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
        //     return factory.GetUrlHelper(actionContext);
        // });
        return services;
    }

}