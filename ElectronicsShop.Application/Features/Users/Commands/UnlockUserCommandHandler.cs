using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ElectronicsShop.Application.Features.Users.Commands;

public class UnlockUserCommandHandler:ResponseHandler, IRequestHandler<UnlockUserCommand, GenericResponse<Unit>>
{
    private readonly UserManager<User> _userManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UnlockUserCommandHandler(UserManager<User> userManager, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GenericResponse<Unit>> Handle(UnlockUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
        {
            return NotFound<Unit>("User not found");
        }

        // Reactivate the user using the domain method
        user.Activate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Success(Unit.Value, "User unlocked successfully");

    }
}