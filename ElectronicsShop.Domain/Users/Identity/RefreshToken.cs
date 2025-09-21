using System.Security.Cryptography;
using ElectronicsShop.Domain.Common;
using ElectronicsShop.Domain.Common.Results;

namespace ElectronicsShop.Domain.Users.Identity;

public sealed class RefreshToken : BaseAuditableEntity
{
    public string Token { get; private set; }
    public Guid UserId { get; private set; }
    public DateTimeOffset ExpiresOnUtc { get; private set; }
    public DateTimeOffset? RevokedOnUtc { get; private set; }
    public bool IsRevoked => RevokedOnUtc != null;
    public bool IsExpired => DateTime.UtcNow >= ExpiresOnUtc;
    public bool IsActive => !IsRevoked && !IsExpired;

    public User? User { get; private set; }
    
    private RefreshToken() { }

    private RefreshToken(Guid userId)
    {
        UserId = userId;
        Token = GenerateTokenString(); 
        ExpiresOnUtc = DateTimeOffset.UtcNow.AddDays(7);
    }
    public static Result<RefreshToken> Create(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return RefreshTokenErrors.UserIdRequired;
        }

        return new RefreshToken(userId);
    }
    public void Revoke()
    {
        // Only revoke a token that is currently active.
        if (!IsActive)
        {
            return;
        }
        RevokedOnUtc = DateTimeOffset.UtcNow;
    }
    private static string GenerateTokenString()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}

