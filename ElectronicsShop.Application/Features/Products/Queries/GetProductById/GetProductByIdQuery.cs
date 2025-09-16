using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Products.Dtos;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(int Id): IRequest<GenericResponse<ProductResponse>>;