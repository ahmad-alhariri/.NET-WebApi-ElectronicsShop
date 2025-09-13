using ElectronicsShop.Api.BaseController;
using ElectronicsShop.Api.Extensions;
using ElectronicsShop.Api.MetaData;
using ElectronicsShop.Application.Features.Products.Commands.CreateProduct;
using ElectronicsShop.Application.Features.Products.Commands.CreateProduct.Images;
using ElectronicsShop.Application.Features.Products.Commands.DeleteProduct;
using ElectronicsShop.Application.Features.Products.Commands.DeleteProduct.DeleteImage;
using ElectronicsShop.Application.Features.Products.Commands.UpdateProduct;
using ElectronicsShop.Application.Features.Products.Commands.UpdateProduct.UpdatePrimaryImage;
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
    
    [HttpGet(ApiRoutes.Products.Search)]
    public async Task<IActionResult> SearchProducts([FromQuery] SearchProductsQuery query)
    {
        var result = await Mediator.Send(query);
        return result.ToActionResult();
    }
    
    [HttpGet(ApiRoutes.Products.LowStock)]
    public async Task<IActionResult> GetLowStockProducts(
        [FromQuery] int threshold = 10,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new GetLowStockProductsQuery(threshold, page, pageSize);
        var result = await Mediator.Send(query);
        return result.ToActionResult();
    }
    
    [HttpGet(ApiRoutes.Products.FeaturedProducts)]
    public async Task<IActionResult> GetFeaturedProducts(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = new GetFeaturedProductsQuery(page, pageSize);
        var result = await Mediator.Send(query);
        return result.ToActionResult();
    }

    [HttpGet(ApiRoutes.Products.NewProducts)]
    public async Task<IActionResult> GetNewProducts(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = new GetNewProductsQuery(page, pageSize);
        var result = await Mediator.Send(query);
        return result.ToActionResult();
    }
    
    
    [HttpGet(ApiRoutes.Products.GetById)]
    public async Task<IActionResult> GetProductById([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetProductByIdQuery(id));
        return result.ToActionResult();
    }
    
    [HttpGet(ApiRoutes.Products.Statistics)]
    public async Task<IActionResult> GetStatistics()
    {
        var result = await Mediator.Send(new GetProductStatisticsQuery());
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


    [HttpPost(ApiRoutes.Products.AddImages)]
    public async Task<IActionResult> AddProductImages([FromForm] AddProductImageCommand command, int id)
    {
        if(id != command.ProductId)
            return BadRequest("Route Id and command ProductId do not match");
        
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    
    [HttpPatch(ApiRoutes.Products.SetPrimaryImage)]
    public async Task<IActionResult> SetPrimaryProductImage(int id, int imageId)
    {
        var command = new SetPrimaryProductImageCommand(id, imageId);
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    
    [HttpDelete(ApiRoutes.Products.RemoveImage)]
    public async Task<IActionResult> DeleteProductImage(int id, int imageId)
    {
        var result = await Mediator.Send(new DeleteProductImageCommand(id, imageId));
        return result.ToActionResult();
    }
    
    [HttpDelete(ApiRoutes.Products.RemoveImages)]
    public async Task<IActionResult> DeleteProductImages(int id, [FromQuery] List<int> imageIds)
    {
        var command = new DeleteProductImagesCommand(id, imageIds);
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    
    [HttpPatch(ApiRoutes.Products.UpdateStock)]
    public async Task<IActionResult> UpdateProductStock(int id, [FromBody] UpdateStockQuantityCommand command)
    {
        if (id != command.ProductId)
            return BadRequest("Route Id and command ProductId do not match");
        
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpPatch(ApiRoutes.Products.UpdatePrice)]
    public async Task<IActionResult> UpdateProductPrice(int id, [FromBody] UpdatePriceCommand command)
    {
        if (id != command.ProductId)
            return BadRequest("Route Id and command ProductId do not match");
        
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    
    [HttpPatch(ApiRoutes.Products.Featured)]
    public async Task<IActionResult> SetFeaturedProduct(int id, [FromBody] SetProductFeaturedStatusCommand command)
    {
        if (id != command.ProductId)
            return BadRequest("Route Id and command ProductId do not match");
        
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    
    [HttpPost(ApiRoutes.Products.AddSpecifications)]
    public async Task<IActionResult> AddProductSpecifications(int id, [FromBody] AddOrUpdateSpecificationsCommand command)
    {
        if(id != command.ProductId)
            return BadRequest("Route Id and command ProductId do not match");
        
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    
    [HttpDelete(ApiRoutes.Products.RemoveSpecification)]
    public async Task<IActionResult> RemoveProductSpecification(int id, string key)
    {
        var command = new RemoveSpecificationCommand(id, key);
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    
    [HttpDelete(ApiRoutes.Products.ClearSpecifications)]
    public async Task<IActionResult> ClearAllProductSpecifications(int id)
    {
        var command = new ClearSpecificationsCommand(id);
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }

}