namespace ElectronicsShop.Domain.Settings;

public class AdminUserSettings
{
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public List<AdminAddressSettings>? Addresses { get; set; }
    public string Password { get; set; } = null!;
    public string Role { get; set; } = "Admin";
}

public class AdminAddressSettings
{
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Country { get; set; } = null!;
}