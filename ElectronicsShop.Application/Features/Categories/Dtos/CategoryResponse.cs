namespace ElectronicsShop.Application.Features.Categories.Dtos;

public sealed record CategoryResponse(
    int Id,
    string CategoryName,
    string Description,
    string ImageUrl,
    int ProductCount,
    DateTime CreatedDate,
    DateTime UpdatedDate);
