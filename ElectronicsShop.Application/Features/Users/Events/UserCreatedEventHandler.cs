using System.Text;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Settings;
using ElectronicsShop.Domain.Users;
using ElectronicsShop.Domain.Users.Events;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;


namespace ElectronicsShop.Application.Features.Users.Events;

public class UserCreatedEventHandler: INotificationHandler<UserCreatedEvent>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;
    private readonly ApiSettings _apiSettings;


    public UserCreatedEventHandler(UserManager<User> userManager, IEmailService emailService, IOptions<ApiSettings> apiSettings)
    {
        _userManager = userManager;
        _emailService = emailService;
        _apiSettings = apiSettings.Value;
    }
    
    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(notification.Email);
        if (user is null)
        {
            // Or log an error, user should not be null here
            return;
        }

        if (_userManager.IsEmailConfirmedAsync(user).Result)
        {
            return;
        }
        //  Generate the token
        var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmationToken));
        
        // var requestAccessor = _httpContextAccessor.HttpContext.Request;
        //
        // // Create the confirmation link
        // var confirmationLink  = requestAccessor.Scheme + "://" + requestAccessor.Host +
        //                         _urlHelper.Action("ConfirmEmail", "Authentication", new { userEmail = notification.Email, token });

        var confirmationLink = $"{_apiSettings.BaseUrl}/api/authentication/confirm-email?userEmail={notification.Email}&token={token}";
        
        var emailBody = $"Welcome! Please <a href='{confirmationLink}'>Click Here:)</a> to confirm your email.";
        
        await _emailService.SendEmailAsync(notification.Email, "Confirm your email", emailBody);
    }
}