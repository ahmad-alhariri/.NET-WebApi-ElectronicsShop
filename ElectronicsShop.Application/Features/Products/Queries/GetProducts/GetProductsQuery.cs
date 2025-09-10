using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Products.Dtos;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    int? CategoryId = null,
    int? BrandId = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    string SortColumn = "createdAt",
    string SortDirection = "desc"
) : IRequest<GenericResponse<List<ProductResponse>>>;