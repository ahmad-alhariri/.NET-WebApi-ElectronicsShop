using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Products.Dtos;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductStatisticsQuery 
    : IRequest<GenericResponse<ProductStatisticsResponse>>;