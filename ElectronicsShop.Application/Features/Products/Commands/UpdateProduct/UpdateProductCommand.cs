using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Products.Dtos;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
    int Id,
    string Name,
    string Description,
    decimal PriceAmount,
    string Sku,
    int CategoryId,
    int BrandId,
    List<SpecificationDto>? Specifications) : IRequest<GenericResponse<int>>; 