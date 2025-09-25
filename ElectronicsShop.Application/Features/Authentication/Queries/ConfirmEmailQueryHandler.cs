using System.Text;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace ElectronicsShop.Application.Features.Authentication.Queries;

public class ConfirmEmailQueryHandler: ResponseHandler, IRequestHandler<ConfirmEmailQuery, GenericResponse<Unit>>
{
    private readonly UserManager<User> _userManager;

    public ConfirmEmailQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    public async Task<GenericResponse<Unit>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
    {
        if (request.UserEmail == null || request.Token == null)
            return BadRequest<Unit>("UserEmail and Token are required");

        var user = await _userManager.FindByEmailAsync(request.UserEmail);
        

        if (user == null)
            return NotFound<Unit>($"User with Email: {request.UserEmail} not found");

        if (user.EmailConfirmed)
            return Conflict<Unit>("Email is already confirmed");

        var decodedTokenBytes = WebEncoders.Base64UrlDecode(request.Token);
        var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes); 
        
        var confirmEmail = await _userManager.ConfirmEmailAsync(user, decodedToken);
        
        if (!confirmEmail.Succeeded)
            return Conflict<Unit>("Failed: " + string.Join(", ", confirmEmail.Errors.Select(e => e.Description)));
        
        return Success(Unit.Value, "Email confirmed successfully");
    }
}