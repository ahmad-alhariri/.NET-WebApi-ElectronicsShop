using ElectronicsShop.Api.BaseController;
using ElectronicsShop.Api.Extensions;
using ElectronicsShop.Api.MetaData;
using ElectronicsShop.Application.Features.Categories.Commands.CreateCategory;
using ElectronicsShop.Application.Features.Categories.Commands.DeleteCategory;
using ElectronicsShop.Application.Features.Categories.Commands.UpdateCategory;
using ElectronicsShop.Application.Features.Categories.Queries.GetCategories;
using ElectronicsShop.Application.Features.Categories.Queries.GetCategoryById;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShop.Api.Controllers;

[ApiController]
public class CategoryController:AppControllerBase
{
    [HttpGet(ApiRoutes.Categories.GetAll)]
    public async Task<IActionResult> GetAllCategories([FromQuery] GetCategoriesQuery query)
    {
        var result = await Mediator.Send(query);
        return result.ToActionResult();
    }
    
    [HttpGet(ApiRoutes.Categories.GetById)]
    public async Task<IActionResult> GetCategoryById([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetCategoryByIdQuery(id));
        return result.ToActionResult();
    }
    
    [HttpPost(ApiRoutes.Categories.Create)]
    public async Task<IActionResult> AddCategory([FromForm] CreateCategoryCommand command)
    {
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    
    [HttpPut(ApiRoutes.Categories.Update)]
    public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryCommand command, int id)
    {
        if (id != command.Id)
            return BadRequest("Route Id and command Id do not match");
        
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    
    [HttpDelete(ApiRoutes.Categories.Delete)]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id)
    {
        var result = await Mediator.Send(new DeleteCategoryCommand(id));
        return result.ToActionResult();
    }
}