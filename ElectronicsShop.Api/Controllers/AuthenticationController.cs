using ElectronicsShop.Api.BaseController;
using ElectronicsShop.Api.Extensions;
using ElectronicsShop.Api.MetaData;
using ElectronicsShop.Application.Features.Authentication.Commands.ChangeUserPassword;
using ElectronicsShop.Application.Features.Authentication.Commands.LogoutUser;
using ElectronicsShop.Application.Features.Authentication.Commands.RefreshExpiredToken;
using ElectronicsShop.Application.Features.Authentication.Commands.RegisterUser;
using ElectronicsShop.Application.Features.Authentication.Commands.SigninUser;
using ElectronicsShop.Application.Features.Authentication.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShop.Api.Controllers;

[ApiController]
public class AuthenticationController : AppControllerBase
{
    [HttpPost(ApiRoutes.Auth.Register)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
    {
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    
    [HttpGet(ApiRoutes.Auth.ConfirmEmail)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery query)
    {
        var result = await Mediator.Send(query);
        return result.ToActionResult();
    }
    
    [HttpPost(ApiRoutes.Auth.SignIn)]
    public async Task<IActionResult> SignInUser([FromBody] SigninUserCommand command)
    {
        var result = await Mediator.Send(command);
        
        // --- Set the Refresh Token in a Secure Cookie ---
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // Prevents client-side script access
            Secure = true,   // Ensures cookie is sent over HTTPS
            SameSite = SameSiteMode.Strict, // Mitigates CSRF attacks
            Expires = result.Data.RefreshTokenDto.ExpireAt
        };
        Response.Cookies.Append("refreshToken", result.Data.RefreshTokenDto.TokenString, cookieOptions);
        return result.ToActionResult();
    }
    
    [HttpPost(ApiRoutes.Auth.RefreshToken)]
    public async Task<IActionResult> RefreshToken([FromBody] string expiredAccessToken)
    {
        // 1. Get the refresh token from the secure cookie
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new { message = "Invalid token." });
        }
        var command = new RefreshTokenCommand(expiredAccessToken, refreshToken);
        var result = await Mediator.Send(command);
        
        // --- Set the new Refresh Token in a Secure Cookie ---
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // Prevents client-side script access
            Secure = true,   // Ensures cookie is sent over HTTPS
            SameSite = SameSiteMode.Strict, // Mitigates CSRF attacks
            Expires = result.Data.RefreshTokenDto.ExpireAt
        };
        Response.Cookies.Append("refreshToken", result.Data.RefreshTokenDto.TokenString, cookieOptions);
        
        return result.ToActionResult();
    }

    [HttpPost(ApiRoutes.Auth.Logout)]
    public async Task<IActionResult> LogoutUser()
    {

        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return BadRequest("No refresh token found.");
        }
        
        var command = new LogoutUserCommand(refreshToken);
        var result = await Mediator.Send(command);
        
        Response.Cookies.Delete("refreshToken");

        return result.ToActionResult();
    }
    
    [Authorize]
    [HttpPost(ApiRoutes.Auth.ChangePassword)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }

    [HttpPost(ApiRoutes.Auth.ForgotPassword)]
    [AllowAnonymous] // This endpoint must be accessible to logged-out users
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    
    [HttpPost(ApiRoutes.Auth.ResetPassword)]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
}