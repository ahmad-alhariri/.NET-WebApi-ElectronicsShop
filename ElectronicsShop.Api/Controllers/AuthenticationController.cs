using ElectronicsShop.Api.BaseController;
using ElectronicsShop.Api.Extensions;
using ElectronicsShop.Api.MetaData;
using ElectronicsShop.Application.Features.Authentication.Commands;
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
}