using System.Text;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Settings;
using ElectronicsShop.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace ElectronicsShop.Application.Features.Authentication.Commands.ChangeUserPassword;

public class ForgotPasswordCommandHandler: ResponseHandler, IRequestHandler<ForgotPasswordCommand, GenericResponse<Unit>>
{
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;
    private readonly ApiSettings _apiSettings;

    public ForgotPasswordCommandHandler(IEmailService emailService, UserManager<User> userManager, IOptions<ApiSettings> apiSettings)
    {
        _emailService = emailService;
        _userManager = userManager;
        _apiSettings = apiSettings.Value;
    }
    
    public async Task<GenericResponse<Unit>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        // SECURITY: Do not reveal if the user was found or not.
        // If a valid user is found, proceed to send the email.
        if (user is not null && user.EmailConfirmed)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            
            // The link must point to your frontend application's reset page
            var resetLink = $"{_apiSettings.BaseUrl}/reset-password?email={user.Email}&token={encodedToken}";

            var emailBody = $"Please reset your password by <a href='{resetLink}'>clicking here</a>.";
            await _emailService.SendEmailAsync(user.Email!, "Reset Password",emailBody);
        }
        
        // Always return success to prevent user enumeration.
        return Success(Unit.Value, "If an account with that email exists, a password reset link has been sent.");
    }
}