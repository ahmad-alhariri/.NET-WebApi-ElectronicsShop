using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Categories.Dtos;
using MediatR;

namespace ElectronicsShop.Application.Features.Categories.Queries.GetCategories;

public sealed record GetCategoriesQuery(
    int Page,
    int PageSize,
    string? SearchTerm,
    string SortColumn = "createdAt",
    string SortDirection = "desc")
    : IRequest<GenericResponse<List<CategoryResponse>>>;