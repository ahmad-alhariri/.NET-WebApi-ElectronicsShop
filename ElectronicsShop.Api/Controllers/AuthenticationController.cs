using ElectronicsShop.Api.BaseController;
using ElectronicsShop.Api.Extensions;
using ElectronicsShop.Api.MetaData;
using ElectronicsShop.Application.Features.Authentication.Commands.RegisterUser;
using ElectronicsShop.Application.Features.Authentication.Commands.SigninUser;
using ElectronicsShop.Application.Features.Authentication.Queries;
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
}