using ElectronicsShop.Application.Common.Models;

using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Domain.Users;
using ElectronicsShop.Domain.Users.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ElectronicsShop.Application.Features.Authentication.Commands.RegisterUser;

public class RegisterUserCommandHandler:ResponseHandler, IRequestHandler<RegisterUserCommand, GenericResponse<Guid>>
{
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(UserManager<User> userManager, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }
    
    
    public async Task<GenericResponse<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null) return BadRequest<Guid>("Email already in use");
        
        // Create the User entity using our domain factory method
        var userResult = User.Create(request.Email, request.FirstName, request.LastName);
        
        if (userResult.IsError)
        {
            return BadRequest<Guid>(userResult.Errors.First().Description);
        }
        
        var user = userResult.Value;
        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // 1. Create the user
            var identityResult = await _userManager.CreateAsync(user, request.Password);
            if (!identityResult.Succeeded)
            {
                return UnprocessableEntity<Guid>(identityResult.Errors.First().Description);
            }

            // 2. Assign the role
            await _userManager.AddToRoleAsync(user, RoleConstants.User);
            
            // 3. If both succeed, commit the transaction
            await transaction.CommitAsync(cancellationToken);

            // Return a successful response
            return Created(user.Id, "User registered successfully, please confirm your email.");
        }
        catch (Exception e)
        {
            // 4. If any exception occurs, explicitly roll back
            await transaction.RollbackAsync(cancellationToken);
            // You can log the exception here
            return InternalServerError<Guid>("An error occurred while registering the user." + e.Message);
        }
        
        
    }
}