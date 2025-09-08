using ElectronicsShop.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;


namespace ElectronicsShop.Api.Extensions;

public static class ActionResultExtensions
{
    public static IActionResult ToActionResult<T>(this GenericResponse<T> response)
    {
        return new ObjectResult(response)
        {
            StatusCode = (int)response.StatusCode
        };
    }
}