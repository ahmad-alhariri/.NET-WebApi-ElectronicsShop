
using ElectronicsShop.Domain.Common.Results;

namespace ElectronicsShop.Domain.Carts;

public static class CartErrors
{
    public static Error QuantityMustBePositive => Error.Validation("Quantity_Must_Be_Positive", "Quantity must be positive");
    public static Error InsufficientStock => Error.Validation("Insufficient_Stock", "Insufficient stock for the requested quantity");
    public static Error ItemNotFound => Error.NotFound("Cart_Item_Not_Found", "Cart item not found");
    public static Error InvalidPrice => Error.Validation("Invalid_Price", "Price must be greater than zero");
    public static Error CartIsEmpty => Error.Validation("Cart_Is_Empty", "Cart is empty");
}