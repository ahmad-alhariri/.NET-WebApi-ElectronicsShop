using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Authentication.Commands.LogoutUser;

public class LogoutUserCommandHandler:ResponseHandler, IRequestHandler<LogoutUserCommand, GenericResponse<Unit>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutUserCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<GenericResponse<Unit>> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        // Find the token in the database
        var refreshToken = await _refreshTokenRepository.GetByTokenStringAsync(request.RefreshToken);
        
        // It's not an error if the token isn't found (it might be expired/invalid already).
        if (refreshToken is null || !refreshToken.IsActive) return BadRequest<Unit>("Token is invalid or already logged out.");
        // Revoke the token using the domain method
        refreshToken.Revoke();

        // Save the change to the database
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success(Unit.Value, "Logged out successfully.");

    }
}