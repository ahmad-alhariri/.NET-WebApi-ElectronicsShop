using ElectronicsShop.Api.BaseController;
using ElectronicsShop.Api.Extensions;
using ElectronicsShop.Api.MetaData;
using ElectronicsShop.Application.Features.Users.Commands;
using ElectronicsShop.Application.Features.Users.Queries.GetUserById;
using ElectronicsShop.Application.Features.Users.Queries.GetUsers;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShop.Api.Controllers;

[ApiController]
public class UserController: AppControllerBase
{
    [HttpGet(ApiRoutes.Users.GetAll)]
    public async Task<IActionResult> GetAll([FromQuery] GetUsersQuery query)
    {
        var result = await Mediator.Send(query);
        return result.ToActionResult();
    }

    [HttpGet(ApiRoutes.Users.GetById)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new GetUserByIdQuery(id));
        return result.ToActionResult();
    }

    [HttpPost(ApiRoutes.Users.Lock)]
    public async Task<IActionResult> Lock([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new LockUserCommand(id));
        return result.ToActionResult();
    }
    
    [HttpPost(ApiRoutes.Users.Unlock)]
    public async Task<IActionResult> Unlock([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new UnlockUserCommand(id));
        return result.ToActionResult();
    }
}