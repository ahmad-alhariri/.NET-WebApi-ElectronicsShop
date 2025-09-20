using ElectronicsShop.Domain.Settings;
using ElectronicsShop.Domain.Users;
using ElectronicsShop.Domain.Users.Address;
using ElectronicsShop.Domain.Users.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace ElectronicsShop.Persistence.DataSeeding;

public static class AdminSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        var adminConfig = scope.ServiceProvider.GetRequiredService<IOptions<AdminUserSettings>>();
        var adminSettings = adminConfig.Value;

        // Create Role if not exists
        if (!await roleManager.RoleExistsAsync(adminSettings.Role))
            await roleManager.CreateAsync(new Role { Name = adminSettings.Role });

        var admin = await userManager.FindByEmailAsync(adminSettings.Email);
        if (admin == null)
        {
            var newAdmin = User.Create(adminSettings.Email, adminSettings.FirstName, adminSettings.LastName,
                adminSettings.UserName, adminSettings.PhoneNumber);

            if (newAdmin.IsError)
            {
                throw new Exception($"Failed to create admin user: {string.Join(", ", newAdmin.Errors.Select(e => e.Description))}");
            }
            
            newAdmin.Value.EmailConfirmed = true;
            var result = await userManager.CreateAsync(newAdmin.Value, adminSettings.Password);

            if (result.Succeeded)
                await userManager.AddToRoleAsync(newAdmin.Value, adminSettings.Role);
                // Attach addresses if provided
                if (adminSettings.Addresses != null && adminSettings.Addresses.Count > 0)
                {
                    foreach (var address in adminSettings.Addresses)
                    {
                        var addressResult = newAdmin.Value.AddAddress(address.Street, address.City, "no-state",address.Country, address.PostalCode);
                        if (addressResult.IsError)
                        {
                            throw new Exception($"Failed to add address to admin user: {string.Join(", ", addressResult.Errors.Select(e => e.Description))}");
                        }
                    }
                }
            
            else
                throw new Exception(
                    $"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}