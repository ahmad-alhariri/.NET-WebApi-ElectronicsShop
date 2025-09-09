namespace ElectronicsShop.Application.Features.Categories.Dtos;

public sealed record CategoryResponse
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ImageUrl { get; init; }
    public int ProductCount { get; init; }
    
    public DateTime CreatedDate { get; init; }
    public DateTime UpdatedDate { get; init; }
}

// (
//     int Id,
//     string CategoryName,
//     string Description,
//     string ImageUrl,
//     int ProductCount,
//     DateTime CreatedDate,
//     DateTime UpdatedDate)