using ElectronicsShop.Domain.Users.Constants;
using ElectronicsShop.Domain.Users.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicsShop.Persistence.DataSeeding;

public static class RoleSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        foreach (var roleName in RoleConstants.AllRoles)
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new Role { Name = roleName });
    }
}