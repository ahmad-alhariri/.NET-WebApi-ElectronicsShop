namespace ElectronicsShop.Application.Features.Carts.Dtos;

public sealed record CartItemResponse
{
    public int CartItemId { get; init; }
    public int ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int Quantity { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
    public decimal LineItemTotal => Price * Quantity;
}