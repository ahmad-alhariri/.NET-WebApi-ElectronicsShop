using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Categories.Dtos;
using MediatR;

namespace ElectronicsShop.Application.Features.Categories.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(int Id): IRequest<GenericResponse<CategoryResponse>>;
