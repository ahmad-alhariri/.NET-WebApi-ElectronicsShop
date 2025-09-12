using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Products.Dtos;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.CreateProduct;

public sealed record AddOrUpdateSpecificationsCommand(int ProductId, List<SpecificationDto>? Specifications): IRequest<GenericResponse<Unit>>;