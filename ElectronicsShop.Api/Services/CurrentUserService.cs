using System.Security.Claims;
using ElectronicsShop.Application.Interfaces.Services;

namespace ElectronicsShop.Api.Services;

public class CurrentUserService:ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    

    public Guid? UserId
    {
        get
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdString, out var userId) ? userId : null;
        }
    }

    public Guid? AnonymousId
    {
        get
        {
            if (_httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue("guestCartId", out var cartIdFromCookie) == true &&
                Guid.TryParse(cartIdFromCookie, out var anonymousId))
            {
                return anonymousId;
            }
            return null;
        }
    }

    public string? UserEmail => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public IEnumerable<string> Roles => _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role)
        .Select(c => c.Value) ?? Enumerable.Empty<string>();

    public async Task<(Guid? userId, Guid? anonymousId)> GetIdentifiers()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        Guid? userId = null;
        Guid? anonymousId = null;

        // Check if the user is authenticated
        if (httpContext?.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userIdClaim, out var id)) userId = id;
        }
        else // If not authenticated, check for the anonymous cart cookie
        {
            if (httpContext.Request.Cookies.TryGetValue("guestCartId", out var cartIdFromCookie) &&
                Guid.TryParse(cartIdFromCookie, out var id))
                anonymousId = id;
        }

        return (userId, anonymousId);
    }

    public void AppendAnonymousId(Guid? anonymousId)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(14)
        };
        _httpContextAccessor.HttpContext?.Response.Cookies.Append("guestCartId", anonymousId!.ToString(), cookieOptions);
    }

    public void RemoveAnonymousId()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("guestCartId");
    }
}