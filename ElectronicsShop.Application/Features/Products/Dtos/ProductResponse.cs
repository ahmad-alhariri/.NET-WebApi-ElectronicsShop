namespace ElectronicsShop.Application.Features.Products.Dtos;

public sealed record ProductResponse
{
    public int Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public decimal Price { get; init; }

    public int StockQuantity { get; init; }

    public string CategoryName { get; init; }

    public List<string> ImagesUrl { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}