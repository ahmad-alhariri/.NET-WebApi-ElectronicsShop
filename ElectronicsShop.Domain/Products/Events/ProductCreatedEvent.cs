using ElectronicsShop.Domain.Common;
using ElectronicsShop.Domain.Common.ValueObjects;

namespace ElectronicsShop.Domain.Products.Events;

public class ProductCreatedEvent: BaseEvent
{
    
    public int ProductId { get; set; }
    
    public string ProductName { get; set; } 
    
    public ProductCreatedEvent(int productId, string productName)
    {
        ProductId = productId;
        ProductName = productName;
    }
}public class ProductStockChangedEvent: BaseEvent
{
    public int ProductId { get; set; }
    public int OldStock { get; set; }
    
    public int Quantity { get; set; }
    
    public ProductStockChangedEvent(int productId ,int oldStock, int quantity)
    {
        ProductId = productId;
        OldStock = oldStock;
        Quantity = quantity;
    }
}
public class ProductLowStockEvent: BaseEvent
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    
    public ProductLowStockEvent(int productId , int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}
public class ProductOutOfStockEvent: BaseEvent
{
    public int ProductId { get; set; }
    
    public ProductOutOfStockEvent(int productId)
    {
        ProductId = productId;
    }
}
public class ProductActivatedEvent: BaseEvent
{
    public int ProductId { get; set; }
    
    public ProductActivatedEvent(int productId)
    {
        ProductId = productId;
    }
}
public class ProductDeactivatedEvent: BaseEvent
{
    public int ProductId { get; set; }
    
    public ProductDeactivatedEvent(int productId)
    {
        ProductId = productId;
    }
}
public class ProductPriceChangedEvent: BaseEvent
{
    public int ProductId { get; set; }
    public Money OldPrice { get; set; }
    public Money NewPrice { get; set; }
    
    public ProductPriceChangedEvent(int productId, Money oldPrice, Money newPrice)
    {
        ProductId = productId;
        OldPrice = oldPrice;
        NewPrice = newPrice;
    }
}