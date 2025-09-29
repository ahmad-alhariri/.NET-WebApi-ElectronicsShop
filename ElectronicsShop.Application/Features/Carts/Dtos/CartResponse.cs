namespace ElectronicsShop.Application.Features.Carts.Dtos;

public sealed record CartResponse
{
    public List<CartItemResponse> Items { get; set; } = new();
    public int TotalItems { get; init; }
    public decimal Subtotal { get; init; }
}