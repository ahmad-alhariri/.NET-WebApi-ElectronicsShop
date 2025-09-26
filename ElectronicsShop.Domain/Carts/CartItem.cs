using ElectronicsShop.Domain.Common;
using ElectronicsShop.Domain.Common.Results;
using ElectronicsShop.Domain.Common.ValueObjects;
using ElectronicsShop.Domain.Products;

namespace ElectronicsShop.Domain.Carts;

public sealed class CartItem:BaseAuditableEntity
{
    public Guid CartId { get; private set; }
    public int ProductId { get; private set; } 
    public int Quantity { get; private set; }
    public Money Price { get; private set; } 
    public Money TotalPrice => new(Price.Amount * Quantity, Price.Currency);
    
    // Navigational properties
    public Cart? Cart { get; private set; }
    public Product? Product { get; private set; }
    
    private CartItem() { } 
    private CartItem(int productId, int quantity, Money price)
    {
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }
    
    internal static Result<CartItem> Create(int productId, int quantity, Money price)
    {
        if (quantity <= 0)
            return CartErrors.QuantityMustBePositive;
        
        if (price.Amount <= 0)
            return CartErrors.InvalidPrice;

        return new CartItem(productId, quantity, price);
    }
    
    internal void IncreaseQuantity(int quantityToAdd)
    {
        if (quantityToAdd > 0)
            Quantity += quantityToAdd;
    }

    internal void SetQuantity(int newQuantity)
    {
        if (newQuantity >= 0)
            Quantity = newQuantity;
    }

}