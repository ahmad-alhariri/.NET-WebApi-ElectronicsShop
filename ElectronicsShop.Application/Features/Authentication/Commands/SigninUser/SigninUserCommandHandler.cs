using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Authentication.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ElectronicsShop.Application.Features.Authentication.Commands.SigninUser;

public class SigninUserCommandHandler:ResponseHandler, IRequestHandler<SigninUserCommand, GenericResponse<TokenResponse>>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public SigninUserCommandHandler(UserManager<User> userManager, ITokenService tokenService, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<GenericResponse<TokenResponse>> Handle(SigninUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Find the user by email
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            // Use a generic error to prevent user enumeration attacks
            return BadRequest<TokenResponse>("Invalid email or password.");
        }
        
        // 2. Check the password
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            return BadRequest<TokenResponse>("Invalid email or password.");
        }
        
        // 3. Check business rules: Is the user verified and active?
        if (!user.EmailConfirmed)
        {
            return BadRequest<TokenResponse>("Email not confirmed. Please verify your email before signing in.");
        }
        
        if (!user.IsActive())
        {
            return BadRequest<TokenResponse>("User account is inactive. Please contact support.");
        }
        
        // 4. Generate JWT and Refresh Token
        var tokenResult = await _tokenService.GenerateJwtTokenAsync(user, cancellationToken);

        if(tokenResult.IsError)
        {
            return UnprocessableEntity<TokenResponse>("Failed to generate tokens: " + string.Join(", ", tokenResult.Errors));
        }
        
        // 5. Record the login activity
        user.RecordLogin();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        // 6. Return the successful token response
        return Success(tokenResult.Value,"Login successful.");
    }
}