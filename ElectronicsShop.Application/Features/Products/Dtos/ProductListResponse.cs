namespace ElectronicsShop.Application.Features.Products.Dtos;

public sealed class ProductListResponse
{
    public int Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }
    
    public string Sku { get; init; }
    public decimal PriceAmount { get; init; }
    public string PriceCurrency { get; init; }
    public int StockQuantity { get; init; }
    public string CategoryName { get; init; }
    public string BrandName { get; init; }
    
    public bool IsActive { get; init; }
    
    public bool IsFeatured { get; init; }
    public bool IsInStock { get; init; }
    public bool IsNew { get; init; }
    
    public string ImageUrl { get; init; }

    public DateTime CreatedDate { get; init; }

    public DateTime UpdatedDate { get; init; }
}