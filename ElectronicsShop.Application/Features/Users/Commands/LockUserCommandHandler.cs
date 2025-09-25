using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ElectronicsShop.Application.Features.Users.Commands;

public class LockUserCommandHandler:ResponseHandler, IRequestHandler<LockUserCommand, GenericResponse<Unit>>
{
    private readonly UserManager<User> _userManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LockUserCommandHandler(UserManager<User> userManager, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<GenericResponse<Unit>> Handle(LockUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
        {
            return NotFound<Unit>("User not found");
        }

        user.Deactivate();
        
        var userTokens = await _refreshTokenRepository.GetActiveTokensByUserIdAsync(user.Id);
        foreach (var token in userTokens)
        {
            token.Revoke();
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Success(Unit.Value, "User locked successfully");

    }
}