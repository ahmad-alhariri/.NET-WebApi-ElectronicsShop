using ElectronicsShop.Api.BaseController;
using ElectronicsShop.Api.Extensions;
using ElectronicsShop.Api.MetaData;
using ElectronicsShop.Application.Features.Carts.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShop.Api.Controllers;

[ApiController]
public class CartController: AppControllerBase
{
    [HttpPost(ApiRoutes.Cart.AddItem)]
    [AllowAnonymous]
    public async Task<IActionResult> AddToCart([FromBody] AddItemToCartCommand command)
    {
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
}