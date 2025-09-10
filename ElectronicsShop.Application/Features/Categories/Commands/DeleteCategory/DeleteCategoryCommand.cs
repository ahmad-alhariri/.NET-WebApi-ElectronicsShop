using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Categories.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand(int Id): IRequest<GenericResponse<bool>>;