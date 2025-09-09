using ElectronicsShop.Api.BaseController;
using ElectronicsShop.Api.Extensions;
using ElectronicsShop.Api.MetaData;
using ElectronicsShop.Application.Features.Categories.Commands.CreateCategory;
using ElectronicsShop.Application.Features.Categories.Commands.DeleteCategory;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShop.Api.Controllers;

[ApiController]
public class CategoryController:AppControllerBase
{
    [HttpPost(ApiRoutes.Categories.Create)]
    public async Task<IActionResult> AddCategory([FromForm] CreateCategoryCommand command)
    {
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