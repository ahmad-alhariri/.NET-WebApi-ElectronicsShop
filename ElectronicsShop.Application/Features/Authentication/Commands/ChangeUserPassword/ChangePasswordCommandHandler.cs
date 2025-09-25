using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Common.Results;
using ElectronicsShop.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ElectronicsShop.Application.Features.Authentication.Commands.ChangeUserPassword;

public class ChangePasswordCommandHandler:ResponseHandler, IRequestHandler<ChangePasswordCommand, GenericResponse<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<User> _userManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public ChangePasswordCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, UserManager<User> userManager, IRefreshTokenRepository refreshTokenRepository)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _userManager = userManager;
        _refreshTokenRepository = refreshTokenRepository;
    }
    
    public async Task<GenericResponse<Unit>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        // 1. Get the current user's ID securely from their claims
        var userId = _currentUserService.UserId;
        if (userId is null)
        {
            return Forbidden<Unit>("User is not authenticated.");
        }
        
        // 2. Find the user in the database
        var user = await _userManager.FindByIdAsync(userId.Value.ToString());
        if (user is null)
        {
            return NotFound<Unit>("User not found.");
        }
        // 3. Use UserManager to handle the password change
        // This method automatically checks the old password and hashes the new one.
        var changePasswordResult = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!changePasswordResult.Succeeded)
        {
            return BadRequest<Unit>(string.Join(", ", changePasswordResult.Errors.Select(e => e.Description)));
        }
        
        // 4.Revoke all existing refresh tokens
        // This logs the user out of all other devices.
        var userTokens = await _refreshTokenRepository.GetActiveTokensByUserIdAsync(user.Id);
        foreach (var token in userTokens)
        {
            token.Revoke();
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Success(Unit.Value, "Password changed successfully.");
    }
}