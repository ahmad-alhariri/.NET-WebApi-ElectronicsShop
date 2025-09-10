using ElectronicsShop.Api.BaseController;
using ElectronicsShop.Api.Extensions;
using ElectronicsShop.Api.MetaData;
using ElectronicsShop.Application.Features.Products.Commands.CreateProduct;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShop.Api.Controllers;

[ApiController]
public class ProductController : AppControllerBase
{
    [HttpPost(ApiRoutes.Products.Create)]
    public async Task<IActionResult> AddProduct([FromForm] CreateProductCommand command)
    {
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    
}