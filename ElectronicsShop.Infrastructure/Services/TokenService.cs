using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ElectronicsShop.Application.Features.Authentication.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Common.Results;
using ElectronicsShop.Domain.Settings;
using ElectronicsShop.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RefreshToken = ElectronicsShop.Domain.Users.Identity.RefreshToken;

namespace ElectronicsShop.Infrastructure.Services;

public class TokenService: ITokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    private readonly JwtSettings _jwtSettings;

    
    public TokenService(IOptions<JwtSettings> jwtSettings,IRefreshTokenRepository refreshTokenRepository, UserManager<User> userManager, IUnitOfWork unitOfWork)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _jwtSettings = jwtSettings.Value;
    }

    
    public async Task<Result<TokenResponse>> GenerateJwtTokenAsync(User user, CancellationToken ct = default)
    {
        // 1. Generate the access token (no change here)
        var accessToken = await GenerateAccessToken(user);

        // 2. Revoke old refresh tokens
        var oldTokens = await _refreshTokenRepository.GetActiveTokensByUserIdAsync(user.Id);
        foreach (var oldToken in oldTokens)
        {
            oldToken.Revoke();
        }
    
        // 3. Create the new RefreshToken domain entity directly
        var refreshToken = RefreshToken.Create(user.Id);

        if (refreshToken.IsError)
        {
            return refreshToken.Errors;
        }
        
        // 4. Add the new token and save all changes
        await _refreshTokenRepository.AddAsync(refreshToken.Value, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        // 5. Create the DTO for the response from the new entity
        var refreshTokenDto = new RefreshTokenDto
        {
            TokenString = refreshToken.Value.Token,
            UserName = user.UserName!,
            ExpireAt = refreshToken.Value.ExpiresOnUtc
        };

        // 6. Return the final response
        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshTokenDto = refreshTokenDto
        };
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            ValidateIssuer = _jwtSettings.ValidateIssuer,
            ValidIssuer = _jwtSettings.Issuer,
            ValidateAudience = _jwtSettings.ValidateAudience,
            ValidAudience = _jwtSettings.Audience,
            ValidateLifetime = false, // Ignore token expiration
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token.");
        }

        return principal;
    }
    
    #region Helper Methods
    private async Task<string> GenerateAccessToken(User user)
    {
        // The secret key from your settings
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

        // The signing credentials using the UTF8-encoded key
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Use HmacSha256 for consistency

        var claims = await GetClaimsAsync(user);
        var securityToken = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
            signingCredentials: credentials
        );
        var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return  accessToken;
    }

    private async Task<List<Claim>> GetClaimsAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.FirstName!),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);
        return claims;
    }

    #endregion
}