namespace ElectronicsShop.Application.Interfaces.Services;

public interface ICurrentUserService
{
    /// <summary>
    /// Gets the unique ID of the currently authenticated user.
    /// Returns null if the user is not authenticated.
    /// </summary>
    Guid? UserId { get; }
    
    /// <summary>
    /// Gets the email of the currently authenticated user.
    /// </summary>
    string? UserEmail { get; }
    
    /// <summary>
    /// Checks if there is an authenticated user.
    /// </summary>
    bool IsAuthenticated { get; }
    
    /// <summary>
    /// Gets the roles of the currently authenticated user.
    /// </summary>
    IEnumerable<string> Roles { get; }
    
    Task<(Guid? userId, Guid? anonymousId)> GetIdentifiers();

    void AppendAnonymousId(Guid? anonymousId);
}