using ElectronicsShop.Api.BaseController;
using ElectronicsShop.Api.Extensions;
using ElectronicsShop.Api.MetaData;
using ElectronicsShop.Application.Features.Brands.Commands.CreateBrand;
using ElectronicsShop.Application.Features.Brands.Commands.DeleteBrand;
using ElectronicsShop.Application.Features.Brands.Commands.UpdateBrand;
using ElectronicsShop.Application.Features.Brands.Queries.GetBrandById;
using ElectronicsShop.Application.Features.Brands.Queries.GetBrands;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsShop.Api.Controllers;

[ApiController]
public class BrandController:AppControllerBase
{
    [HttpGet(ApiRoutes.Brands.GetAll)]
    public async Task<IActionResult> GetAllBrands([FromQuery] GetBrandsQuery query)
    {
        var result = await Mediator.Send(query);
        return result.ToActionResult();
    }
    
    [HttpGet(ApiRoutes.Brands.GetById)]
    public async Task<IActionResult> GetBrandById([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetBrandByIdQuery(id));
        return result.ToActionResult();
    }
    
    [HttpPost(ApiRoutes.Brands.Create)]
    public async Task<IActionResult> CreateBrand([FromBody] CreateBrandCommand command)
    {
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }
    
    [HttpPut(ApiRoutes.Brands.Update)]
    public async Task<IActionResult> UpdateBrand( [FromBody] UpdateBrandCommand command)
    {
        var result = await Mediator.Send(command);
        return result.ToActionResult();
    }

    [HttpDelete(ApiRoutes.Brands.Delete)]
    public async Task<IActionResult> DeleteBrand([FromRoute] int id)
    {
        var result = await Mediator.Send(new DeleteBrandCommand(id));
        return result.ToActionResult();
    }
}