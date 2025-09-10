using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ElectronicsShop.Application.Features.Products.Commands.CreateProduct;

public sealed record CreateProductCommand( 
    string Name,
    string Description,
    decimal PriceAmount,
    string PriceCurrency,
    int StockQuantity,
    string Sku,
    int CategoryId,
    int BrandId,
    List<SpecificationDto>? Specifications, 
    List<IFormFile>? Images
    )
    : IRequest<GenericResponse<int>>;