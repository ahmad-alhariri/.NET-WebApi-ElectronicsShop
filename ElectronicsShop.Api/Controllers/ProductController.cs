using ElectronicsShop.Api.BaseController;
using ElectronicsShop.Api.Extensions;
using ElectronicsShop.Api.MetaData;
using ElectronicsShop.Application.Features.Products.Commands.CreateProduct;
using ElectronicsShop.Application.Features.Products.Commands.DeleteProduct;
using ElectronicsShop.Application.Features.Products.Commands.UpdateProduct;
using ElectronicsShop.Application.Features.Products.Queries.GetProductById;
using ElectronicsShop.Application.Features.Products.Queries.GetProducts;
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
    
    [HttpGet(ApiRoutes.Products.GetAll)]
    public async Task<IActionResult> GetAllProducts([FromQuery] GetProductsQuery query)
    {
        var result = await Mediator.Send(query);
        return result.ToActionResult();
    }
    
    [HttpGet(ApiRoutes.Products.GetById)]
    public async Task<IActionResult> GetProductById([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetProductByIdQuery(id));
        return result.ToActionResult();
    }
    
    [HttpPut(ApiRoutes.Products.Update)]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.Id)
            return BadRequest("Route Id and command Id do not match");
        
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    
    [HttpDelete(ApiRoutes.Products.Delete)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await Mediator.Send(new DeleteProductCommand(id));
        return result.ToActionResult();
    }
    
}