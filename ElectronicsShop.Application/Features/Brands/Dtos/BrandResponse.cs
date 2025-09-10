namespace ElectronicsShop.Application.Features.Brands.Dtos;

public sealed record  BrandResponse
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string? LogoUrl { get; init; }
    public int ProductCount { get; init; }
    
    public DateTime CreatedDate { get; init; }
    public DateTime UpdatedDate { get; init; }
}