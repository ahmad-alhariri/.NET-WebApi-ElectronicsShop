using ElectronicsShop.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ElectronicsShop.Application.Features.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(int Id, string? Name, string? Description, IFormFile? ImageFile):IRequest<GenericResponse<int>>;