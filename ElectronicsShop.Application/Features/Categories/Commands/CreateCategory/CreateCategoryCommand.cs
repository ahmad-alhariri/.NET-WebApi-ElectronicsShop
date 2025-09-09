using ElectronicsShop.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ElectronicsShop.Application.Features.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(
    string CategoryName,
    string Description,
    IFormFile ImageFile) : IRequest<GenericResponse<int>>;