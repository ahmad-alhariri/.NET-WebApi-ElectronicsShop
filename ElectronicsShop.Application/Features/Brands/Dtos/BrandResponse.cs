namespace ElectronicsShop.Application.Features.Brands.Dtos;

public sealed record BrandResponse(int Id, string Name, string LogoUrl, int ProductCount, DateTime CreatedDate, DateTime UpdatedDate);