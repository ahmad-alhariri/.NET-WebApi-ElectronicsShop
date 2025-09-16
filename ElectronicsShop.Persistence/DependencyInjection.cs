
using ElectronicsShop.Application.Interfaces;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Persistence.DataContext;
using ElectronicsShop.Persistence.Interceptors;
using ElectronicsShop.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicsShop.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);

        AddDbContext(services,configuration);
        AddRepositories(services);
        return services;
    }
    
    private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Register the Auditable Entity Interceptor
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        
        // 2. Register the ApplicationDbContext
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }
            options.UseSqlServer(connectionString);
            
            // Add the interceptor to the DbContext options
            options.AddInterceptors(sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>());
        });
        
        // 3. Scoped IApplicationDbContext
        // This allows injecting the interface, which is useful for testing.
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        
        

        // services.AddScoped<ApplicationDbContextInitialiser>();
    }
    
    private static void AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork))
            .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddScoped<IBrandRepository,BrandRepository>()
            .AddScoped<ICategoryRepository,CategoryRepository>()
            .AddScoped<IProductRepository,ProductRepository>();
    }
}