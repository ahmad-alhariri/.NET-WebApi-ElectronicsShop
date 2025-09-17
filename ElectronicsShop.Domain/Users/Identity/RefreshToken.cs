using ElectronicsShop.Domain.Common;

namespace ElectronicsShop.Domain.Users.Identity;

public sealed class RefreshToken : BaseEntity
{
    public string Token { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    // Navigation property
    public User User { get; private set; }

    private RefreshToken() { }

    private RefreshToken(Guid userId)
    {
        UserId = userId;
        Token = GenerateToken();
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddDays(7); // 7 days expiry
    }

    public static RefreshToken Create(Guid userId)
    {
        return new RefreshToken(userId);
    }

    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
    }

    private static string GenerateToken()
    {
        var randomBytes = new byte[64];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
