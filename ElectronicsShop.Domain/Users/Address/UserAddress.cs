using ElectronicsShop.Domain.Common;
using ElectronicsShop.Domain.Common.Results;
using ElectronicsShop.Domain.Identity.User;

namespace ElectronicsShop.Domain.Users.Address;

public sealed class UserAddress : BaseEntity
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }
    public string? PostalCode { get; private set; }
    public bool IsDefault { get; private set; }
    public Guid UserId { get; private set; }

    // Navigation property
    public User User { get; private set; }

    private UserAddress() { }

    private UserAddress(string street, string city, string state, string country, 
        string? postalCode, Guid userId)
    {
        Street = street.Trim();
        City = city.Trim();
        State = state.Trim();
        Country = country.Trim();
        PostalCode = postalCode?.Trim();
        UserId = userId;
        IsDefault = false;
    }

    public static Result<UserAddress> Create(string street, string city, string state, 
        string country, string? postalCode, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(street))
            return UserErrors.StreetRequired;

        if (string.IsNullOrWhiteSpace(city))
            return UserErrors.CityRequired;

        if (string.IsNullOrWhiteSpace(state))
            return UserErrors.StateRequired;

        if (string.IsNullOrWhiteSpace(country))
            return UserErrors.CountryRequired;
        

        return new UserAddress(street, city, state, country, postalCode, userId);
    }

    public void SetAsDefault() => IsDefault = true;
    public void UnsetAsDefault() => IsDefault = false;

    public string GetFormattedAddress() => $"{Street}, {City}, {State} {PostalCode}, {Country}";

    public Result<Updated> UpdateAddress(string street, string city, string state, 
        string country, string postalCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            return UserErrors.StreetRequired;

        if (string.IsNullOrWhiteSpace(city))
            return UserErrors.CityRequired;

        if (string.IsNullOrWhiteSpace(state))
            return UserErrors.StateRequired;

        if (string.IsNullOrWhiteSpace(country))
            return UserErrors.CountryRequired;

        Street = street.Trim();
        City = city.Trim();
        State = state.Trim();
        Country = country.Trim();
        PostalCode = postalCode.Trim();

        return Result.Updated;
    }
}