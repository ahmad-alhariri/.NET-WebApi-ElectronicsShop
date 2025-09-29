using ElectronicsShop.Domain.Common.Results;
using ElectronicsShop.Domain.Common.ValueObjects;
using ElectronicsShop.Domain.Products;

namespace ElectronicsShop.Domain.Carts;

public sealed class Cart
{
    public Guid Id { get; private set; }
    public Guid? UserId { get; private set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    private readonly List<CartItem> _items = new();
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    public Money Subtotal => new(_items.Sum(i => i.TotalPrice.Amount));
    public int TotalItems => _items.Sum(i => i.Quantity);
    public bool IsEmpty => !_items.Any();
    
    private Cart() { }

    private Cart(Guid? userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
    }

       public static Cart Create(Guid? userId = null)
    {
        return new Cart(userId);
    }

    public Result<Success> AddItem(Product product, int quantity)
    {
        if (quantity <= 0)
            return CartErrors.QuantityMustBePositive;

        var existingItem = _items.FirstOrDefault(i => i.ProductId == product.Id);
        var totalQuantityRequired = (existingItem?.Quantity ?? 0) + quantity;

        if (product.StockQuantity < totalQuantityRequired)
            return CartErrors.InsufficientStock;

        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            var newItemResult = CartItem.Create(product.Id, quantity, product.Price);
            if (newItemResult.IsError)
                return newItemResult.Errors;
            
            _items.Add(newItemResult.Value);
        }
        
        return Result.Success;
    }
    
    public Result<Updated> RemoveItem(int productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null) return CartErrors.ItemNotFound;

        _items.Remove(item);
        UpdatedDate = DateTime.UtcNow;
        return Result.Updated;
    }
    public Result<Deleted> Clear()
    {
        if (IsEmpty) return CartErrors.CartIsEmpty;

        _items.Clear();
        UpdatedDate = DateTime.UtcNow;
        return Result.Deleted;
    }
    
    public Result<Success> UpdateItemQuantity(Product product, int newQuantity)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == product.Id);
        if (item is null) return CartErrors.ItemNotFound;

        if (newQuantity >= 0)
        {
            if (newQuantity == 0)
            {
                _items.Remove(item);
            }
            else
            {
                item.SetQuantity(newQuantity);
            }
        }
        else
        {
            return CartErrors.QuantityMustBePositive;
        }
        UpdatedDate = DateTime.UtcNow;
        return Result.Success;
    }

    public Result<Updated> AssignToUser(Guid userId)
    {
        if (UserId.HasValue)
            return CartErrors.ItemNotFound;
            
        UserId = userId;
        UpdatedDate = DateTime.UtcNow;
        return Result.Updated;
    }
    public void MergeWithAddItem(Product product, int quantity)
    {
        var existingItem = _items.FirstOrDefault(i => i.ProductId == product.Id);
        
        if (existingItem is not null)
        {
            var totalQuantityRequired = existingItem.Quantity + quantity;
                
            // Validate stock availability
            if (totalQuantityRequired <= product.StockQuantity)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                existingItem.SetQuantity(product.StockQuantity);
            }
            
        }
        else
        {
            var newItemResult = CartItem.Create(product.Id, quantity, product.Price);
            _items.Add(newItemResult.Value);
        }
        

    }


    

    
}
