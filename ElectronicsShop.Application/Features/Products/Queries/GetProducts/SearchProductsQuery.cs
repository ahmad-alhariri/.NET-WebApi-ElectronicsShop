using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Products.Dtos;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Queries.GetProducts;

public sealed record SearchProductsQuery(
    string Term,
    int MaxResults = 10
): IRequest<GenericResponse<IReadOnlyList<ProductSearchDto>>>;