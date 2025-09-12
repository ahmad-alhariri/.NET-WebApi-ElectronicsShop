using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Products.Dtos;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Queries.GetProducts;

public sealed record GetLowStockProductsQuery(
    int Threshold = 10,
    int Page = 1,
    int PageSize = 20
) : IRequest<GenericResponse<List<ProductResponse>>>;