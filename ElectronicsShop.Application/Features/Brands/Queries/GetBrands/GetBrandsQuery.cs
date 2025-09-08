using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Brands.Dtos;
using MediatR;

namespace ElectronicsShop.Application.Features.Brands.Queries.GetBrands;

public sealed record GetBrandsQuery(int Page,
    int PageSize,
    string? SearchTerm,
    string SortColumn = "createdAt",
    string SortDirection = "desc"): IRequest<GenericResponse<List<BrandResponse>>>;