using System.Security.Claims;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Authentication.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Common.Results;
using ElectronicsShop.Domain.Identity.User;
using ElectronicsShop.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ElectronicsShop.Application.Features.Authentication.Commands.RefreshExpiredToken;

public class RefreshTokenCommandHandler:ResponseHandler, IRequestHandler<RefreshTokenCommand, GenericResponse<TokenResponse>>
{
    private readonly UserManager<User> _userManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(UserManager<User> userManager, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, ITokenService tokenService)
    {
        _userManager = userManager;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }
    
    public async Task<GenericResponse<TokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate the expired access token and get the user's claims
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.ExpiredAccessToken);
        if (principal is null)
        {
            return BadRequest<TokenResponse>("Invalid access token.");
        }
        
        // 2. Find the user associated with the expired token
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
        {
            return Conflict<TokenResponse>("Invalid token: User ID not found in token claims.");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return NotFound<TokenResponse>("User not found.");
        }
        
        // 3. Find the refresh token in the database
        var oldToken = await _refreshTokenRepository.GetByTokenStringForUserAsync(request.RefreshTokenString, user.Id);
        if (oldToken is null || !oldToken.IsActive)
        {
            return NotFound<TokenResponse>("Refresh token not found or inactive.");
        }
        
        // 5. Revoke the old refresh token
        oldToken.Revoke();
        
        // 6. Generate a new set of tokens
        var tokenResult = await _tokenService.GenerateJwtTokenAsync(user, cancellationToken);
        if(tokenResult.IsError)
        {
            return UnprocessableEntity<TokenResponse>(tokenResult.Errors.First().Description);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        // 7. Return the new tokens
        return Success(tokenResult.Value, "Token refreshed successfully.");
        
    }
}