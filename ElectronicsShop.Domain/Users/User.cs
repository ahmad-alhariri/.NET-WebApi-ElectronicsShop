using ElectronicsShop.Domain.Common;
using ElectronicsShop.Domain.Common.Results;
using ElectronicsShop.Domain.Identity.User;
using ElectronicsShop.Domain.Users.Address;
using ElectronicsShop.Domain.Users.Enums;
using ElectronicsShop.Domain.Users.Events;
using ElectronicsShop.Domain.Users.Identity;
using Microsoft.AspNetCore.Identity;

namespace ElectronicsShop.Domain.Users;

public sealed class User : IdentityUser<Guid>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public UserStatus Status { get; private set; }
    public DateTime? LastLoginDate { get; private set; }

    

    // Address management
    private readonly List<UserAddress> _addresses = new();
    public IReadOnlyList<UserAddress> Addresses => _addresses.AsReadOnly();

    // Refresh tokens
    private readonly List<RefreshToken> _refreshTokens = new();
    public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    // Domain events
    private readonly List<BaseEvent> _domainEvents = new();
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    // Audit properties
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }


    #region Constructors
    private User() { }

    private User(string email, string firstName, string lastName, string userName)
    {
        Email = email.ToLower().Trim();
        NormalizedEmail = email.ToUpper().Trim();
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        UserName = userName.ToLower().Trim();
        NormalizedUserName = userName.ToUpper().Trim();
        Status = UserStatus.Active;

    }
    #endregion

    #region Factory Methods
    public static Result<User> Create(string email, string firstName, string lastName, string? userName = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            return UserErrors.EmailRequired;

        if (!IsValidEmail(email))
            return UserErrors.InvalidEmail;

        if (string.IsNullOrWhiteSpace(firstName))
            return UserErrors.FirstNameRequired;

        if (string.IsNullOrWhiteSpace(lastName))
            return UserErrors.LastNameRequired;

        var finalUserName = userName?.Trim() ?? GenerateUserNameFromEmail(email);

        if (!IsValidUserName(finalUserName))
            return UserErrors.InvalidUserName;

        var user = new User(email, firstName, lastName, finalUserName);
        user.AddDomainEvent(new UserCreatedEvent(user.Id, user.Email!, user.FirstName, user.LastName));

        return user;
    }
    #endregion

    #region Profile Management
    public Result<Updated> UpdateProfile(string firstName, string lastName, string? phoneNumber = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return UserErrors.FirstNameRequired;

        if (string.IsNullOrWhiteSpace(lastName))
            return UserErrors.LastNameRequired;
        

        var oldFirstName = FirstName;
        var oldLastName = LastName;

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        
        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            if (!IsValidPhoneNumber(phoneNumber))
                return UserErrors.InvalidPhoneNumber;
            
            PhoneNumber = phoneNumber.Trim();
        }
        
        return Result.Updated;
    }
    
    #endregion

    #region Address Management
    public Result<Success> AddAddress(string street, string city, string state, string country, 
        string postalCode, AddressType type, bool isDefault = false)
    {
        var addressResult = UserAddress.Create(street, city, state, country, postalCode, type, Id);
        if (!addressResult.IsSuccess)
            return addressResult.Errors.First();

        var address = addressResult.Value;

        // If this is the first address or marked as default, set it as default
        if (!_addresses.Any() || isDefault)
        {
            // Remove default flag from other addresses of the same type
            foreach (var existingAddress in _addresses.Where(a => a.Type == type))
            {
                existingAddress.UnsetAsDefault();
            }
            
            address.SetAsDefault();
        }

        _addresses.Add(address);

        return Result.Success;
    }

    public Result<Success> RemoveAddress(int addressId)
    {
        var address = _addresses.FirstOrDefault(a => a.Id == addressId);
        if (address == null)
            return UserErrors.AddressNotFound;

        _addresses.Remove(address);

        return Result.Success;
    }

    public Result<Success> SetDefaultAddress(int addressId, AddressType type)
    {
        var address = _addresses.FirstOrDefault(a => a.Id == addressId);
        if (address == null)
            return UserErrors.AddressNotFound;

        if (address.Type != type)
            return UserErrors.AddressTypeMismatch;

        // Remove default flag from other addresses of the same type
        foreach (var existingAddress in _addresses.Where(a => a.Type == type && a.Id != addressId))
        {
            existingAddress.UnsetAsDefault();
        }

        address.SetAsDefault();
        return Result.Success;
    }
    #endregion

    #region Authentication & Security
    public Result<Success> RecordLogin()
    {
        LastLoginDate = DateTime.UtcNow;
        return Result.Success;
    }
    #endregion



    #region Status Management
    public Result<Success> Activate()
    {
        if (Status == UserStatus.Active)
            return Result.Success;

        Status = UserStatus.Active;
        return Result.Success;
    }

    public Result<Success> Deactivate()
    {
        if (Status == UserStatus.Inactive)
            return Result.Success;

        Status = UserStatus.Inactive;
        return Result.Success;
    }

    public Result<Success> Suspend()
    {
        Status = UserStatus.Suspended;
        return Result.Success;
    }
    #endregion

    #region Helper Methods
    public string GetFullName() => $"{FirstName} {LastName}";
    
    public UserAddress? GetDefaultAddress(AddressType type) => 
        _addresses.FirstOrDefault(a => a.Type == type && a.IsDefault);

    public bool IsActive() => Status == UserStatus.Active;
    
    public void AddDomainEvent(BaseEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void RemoveDomainEvent(BaseEvent domainEvent) => _domainEvents.Remove(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
    #endregion

    #region Validation Methods
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidUserName(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName) || userName.Length < 3 || userName.Length > 50)
            return false;

        return userName.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '.');
    }

    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        // Simple phone number validation - you might want to use a more sophisticated library
        return !string.IsNullOrWhiteSpace(phoneNumber) && 
               phoneNumber.All(c => char.IsDigit(c) || c == '+' || c == '-' || c == ' ' || c == '(' || c == ')');
    }

    private static string GenerateUserNameFromEmail(string email)
    {
        var localPart = email.Split('@')[0];
        return localPart.ToLower().Replace(".", "").Replace("+", "");
    }
    #endregion
}