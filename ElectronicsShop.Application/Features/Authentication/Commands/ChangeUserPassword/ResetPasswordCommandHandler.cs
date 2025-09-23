using System.Text;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace ElectronicsShop.Application.Features.Authentication.Commands.ChangeUserPassword;

public class ResetPasswordCommandHandler:ResponseHandler, IRequestHandler<ResetPasswordCommand,GenericResponse<Unit>>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<GenericResponse<Unit>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user is null)
        {
            // Don't reveal user non-existence, use a generic error
            return BadRequest<Unit>("Invalid token or email.");
        }

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

        var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);

        if (!result.Succeeded)
        {
            return BadRequest<Unit>(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return Success(Unit.Value,"Password has been reset successfully.");
    }
}