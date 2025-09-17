using ElectronicsShop.Domain.Common.Results;

namespace ElectronicsShop.Domain.Identity.User;

public static class UserErrors
{
    // Basic validation errors
    public static Error EmailRequired => Error.Validation(
        "User.EmailRequired", "Email address is required");
    
    public static Error InvalidEmail => Error.Validation(
        "User.InvalidEmail", "Invalid email address format");
    
    public static Error FirstNameRequired => Error.Validation(
        "User.FirstNameRequired", "First name is required");
    
    public static Error LastNameRequired => Error.Validation(
        "User.LastNameRequired", "Last name is required");
    
    public static Error InvalidUserName => Error.Validation(
        "User.InvalidUserName", "Username must be 3-50 characters and contain only letters, numbers, _, -, or .");
    
    public static Error InvalidDateOfBirth => Error.Validation(
        "User.InvalidDateOfBirth", "Date of birth cannot be in the future");
    
    public static Error InvalidPhoneNumber => Error.Validation(
        "User.InvalidPhoneNumber", "Invalid phone number format");
    
    public static Error InvalidCurrency => Error.Validation(
        "User.InvalidCurrency", "Invalid currency code");
    
    // Profile related errors
    public static Error ProfileImageUrlRequired => Error.Validation(
        "User.ProfileImageUrlRequired", "Profile image URL is required");
    
    public static Error InvalidProfileImageUrl => Error.Validation(
        "User.InvalidProfileImageUrl", "Invalid profile image URL format");
    
    // Address related errors
    public static Error StreetRequired => Error.Validation(
        "User.StreetRequired", "Street address is required");
    
    public static Error CityRequired => Error.Validation(
        "User.CityRequired", "City is required");
    
    public static Error StateRequired => Error.Validation(
        "User.StateRequired", "State is required");
    
    public static Error CountryRequired => Error.Validation(
        "User.CountryRequired", "Country is required");
    
    public static Error PostalCodeRequired => Error.Validation(
        "User.PostalCodeRequired", "Postal code is required");
    
    public static Error AddressNotFound => Error.NotFound(
        "User.AddressNotFound", "Address not found");
    
    public static Error AddressTypeMismatch => Error.Validation(
        "User.AddressTypeMismatch", "Address type does not match the requested type");
    
    // Authentication & Security errors
    public static Error InvalidRefreshToken => Error.Validation(
        "User.InvalidRefreshToken", "Invalid refresh token");
    
    public static Error RefreshTokenNotActive => Error.Validation(
        "User.RefreshTokenNotActive", "Refresh token is not active");
    
    // Loyalty program errors
    public static Error InvalidLoyaltyPoints => Error.Validation(
        "User.InvalidLoyaltyPoints", "Loyalty points must be greater than zero");
    
    public static Error InsufficientLoyaltyPoints => Error.Validation(
        "User.InsufficientLoyaltyPoints", "Insufficient loyalty points balance");
    
    // Role related errors
    public static Error RoleNameRequired => Error.Validation(
        "User.RoleNameRequired", "Role name is required");
    
    public static Error RoleDescriptionRequired => Error.Validation(
        "User.RoleDescriptionRequired", "Role description is required");
    
    // Authentication specific errors
    public static Error UserNotFound => Error.NotFound(
        "User.NotFound", "User not found");
    
    public static Error UserNotFoundById(int id) => Error.NotFound(
        "User.NotFoundById", $"User with ID {id} not found");
    
    public static Error UserNotFoundByEmail(string email) => Error.NotFound(
        "User.NotFoundByEmail", $"User with email '{email}' not found");
    
    public static Error UserAlreadyExists => Error.Conflict(
        "User.AlreadyExists", "User already exists");
    
    public static Error EmailAlreadyExists => Error.Conflict(
        "User.EmailAlreadyExists", "Email address is already registered");
    
    public static Error UserNameAlreadyExists => Error.Conflict(
        "User.UserNameAlreadyExists", "Username is already taken");
    
    public static Error InvalidCredentials => Error.Validation(
        "User.InvalidCredentials", "Invalid email or password");
    
    public static Error AccountLocked => Error.Validation(
        "User.AccountLocked", "Account is temporarily locked due to multiple failed login attempts");
    
    public static Error AccountNotActive => Error.Validation(
        "User.AccountNotActive", "Account is not active");
    
    public static Error AccountSuspended => Error.Validation(
        "User.AccountSuspended", "Account is suspended");
    
    public static Error EmailNotVerified => Error.Validation(
        "User.EmailNotVerified", "Email address is not verified");
    
    public static Error WeakPassword => Error.Validation(
        "User.WeakPassword", "Password does not meet security requirements");
    
    public static Error PasswordMismatch => Error.Validation(
        "User.PasswordMismatch", "Passwords do not match");
    
    public static Error CurrentPasswordIncorrect => Error.Validation(
        "User.CurrentPasswordIncorrect", "Current password is incorrect");
    
    // Token related errors
    public static Error InvalidToken => Error.Validation(
        "User.InvalidToken", "Invalid or expired token");
    
    public static Error TokenExpired => Error.Validation(
        "User.TokenExpired", "Token has expired");
    
    public static Error InvalidVerificationCode => Error.Validation(
        "User.InvalidVerificationCode", "Invalid verification code");
    
    public static Error VerificationCodeExpired => Error.Validation(
        "User.VerificationCodeExpired", "Verification code has expired");
    
    // Two-factor authentication errors
    public static Error TwoFactorRequired => Error.Validation(
        "User.TwoFactorRequired", "Two-factor authentication is required");
    
    public static Error InvalidTwoFactorCode => Error.Validation(
        "User.InvalidTwoFactorCode", "Invalid two-factor authentication code");
    
    // Password reset errors
    public static Error PasswordResetTokenInvalid => Error.Validation(
        "User.PasswordResetTokenInvalid", "Password reset token is invalid or expired");
    
    public static Error TooManyPasswordResetAttempts => Error.Validation(
        "User.TooManyPasswordResetAttempts", "Too many password reset attempts. Please try again later");
}
