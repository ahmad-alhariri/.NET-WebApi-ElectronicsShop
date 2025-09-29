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
    
    [HttpPut(ApiRoutes.Cart.UpdateItem)]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemCommand command)
    {
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    
    [HttpDelete(ApiRoutes.Cart.RemoveItem)]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateCartItem([FromBody] RemoveCartItemCommand command)
    {
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    
    [HttpDelete(ApiRoutes.Cart.ClearCart)]
    [AllowAnonymous]
    public async Task<IActionResult> ClearCart()
    {
        var result = await Mediator.Send(new ClearCartCommand());
        return result.ToActionResult();
    }
}