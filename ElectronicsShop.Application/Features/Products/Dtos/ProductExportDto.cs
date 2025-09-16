namespace ElectronicsShop.Application.Features.Products.Dtos;

public sealed class ProductExportDto
{
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public string Currency { get; init; }
    public int StockQuantity { get; init; }
    public string Sku { get; init; }
    public int CategoryId { get; init; }
    public string Category { get; init; }
    public int BrandId { get; init; }
    public string Brand { get; init; }
    public bool IsActive { get; init; }
    public bool IsFeatured { get; init; }
}