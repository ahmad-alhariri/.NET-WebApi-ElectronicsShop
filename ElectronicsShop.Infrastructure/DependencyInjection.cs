using System.Text;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Infrastructure.Services;
using ElectronicsShop.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ElectronicsShop.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind settings first
        BindSettings(services, configuration);
        
        // Register JWT security services
        AddJwtSecurityServices(services, configuration);
        
        // Register application services
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IBulkProductService, BulkProductService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
    
    
    // Configure and bind settings
    private static void BindSettings(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CurrencySettings>(configuration.GetSection("CurrencySettings"));
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<AdminUserSettings>(configuration.GetSection("AdminUser"));
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));
    }
    
    // Register JWT security services
    private static void AddJwtSecurityServices(IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        
        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtSettings!.ValidateIssuer,
                    ValidIssuers = new[] { jwtSettings.Issuer },
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    ValidAudience = jwtSettings.Audience,
                    ValidateAudience = jwtSettings.ValidateAudience,
                    ValidateLifetime = jwtSettings.ValidateLifeTime,
                    // Add clock skew to handle minor time differences
                    ClockSkew = TimeSpan.Zero
                };
            });
    }


}