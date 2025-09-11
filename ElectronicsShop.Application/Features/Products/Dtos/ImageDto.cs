namespace ElectronicsShop.Application.Features.Products.Dtos;

public sealed class ImageDto
{
    public int Id { get; init; }
    public string Url { get; init; }
    public bool IsPrimary { get; init; }
}