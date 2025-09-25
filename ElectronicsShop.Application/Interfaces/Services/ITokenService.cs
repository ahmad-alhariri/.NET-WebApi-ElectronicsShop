using System.Security.Claims;
using ElectronicsShop.Application.Features.Authentication.Dtos;
using ElectronicsShop.Domain.Common.Results;
using ElectronicsShop.Domain.Users;

namespace ElectronicsShop.Application.Interfaces.Services;

public interface ITokenService
{
    Task<Result<TokenResponse>> GenerateJwtTokenAsync(User user, CancellationToken ct = default);

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}