using ElectronicsShop.Domain.Common;
using ElectronicsShop.Domain.Common.Results;
using ElectronicsShop.Domain.Common.ValueObjects;
using ElectronicsShop.Domain.Products.Brands;
using ElectronicsShop.Domain.Products.Categories;
using ElectronicsShop.Domain.Products.Events;

namespace ElectronicsShop.Domain.Products;

public sealed class Product : BaseAuditableEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int StockQuantity { get; private set; }
    public string Sku { get; private set; }
    public bool IsActive { get; private set; }

    public bool IsFeatured { get; private set; }
    
    
    // Value objects
    public Money Price { get; private set; }
    
    private readonly Dictionary<string, string> _specifications = new();
    public IReadOnlyDictionary<string, string> Specifications => _specifications;

    // Foreign keys
    public int CategoryId { get; private set; }
    public int BrandId { get; private set; }

    // Navigation
    public Category? Category { get; private set; }
    public Brand? Brand { get; private set; }

    private readonly List<ProductImage> _images = new();
    public IEnumerable<ProductImage> Images => _images.AsReadOnly();


    #region Constructors

    private Product() { }

    private Product(string name, string description, Money price, int stockQuantity,
        string sku, int categoryId, int brandId, Dictionary<string, string>? initialSpecifications = null)
    {
        Name = name.Trim();
        Description = description.Trim();
        Price = price;
        StockQuantity = stockQuantity;
        Sku = sku.ToUpper().Trim();
        CategoryId = categoryId;
        BrandId = brandId;
        IsActive = true;
        
        if (initialSpecifications is not null)
        {
            _specifications = initialSpecifications;
        }
    }

    #endregion


    #region Factory Method and Update Details

    public static Result<Product> Create(string name, string description, Money price,
        int stockQuantity, string sku, int categoryId, int brandId)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ProductErrors.NameRequired;

        if (string.IsNullOrWhiteSpace(description))
            return ProductErrors.DescriptionRequired;

        if (price.Amount <= 0)
            return ProductErrors.PriceGreaterThanZero;

        if (stockQuantity < 0)
            return ProductErrors.StockQuantityGreaterThanZero;

        if (string.IsNullOrWhiteSpace(sku))
            return ProductErrors.SkuRequired;

        if (categoryId <= 0)
            return ProductErrors.CategoryRequired;

        if (brandId <= 0)
            return ProductErrors.BrandRequired;

        var product = new Product(
            name,
            description,
            price,
            stockQuantity,
            sku,
            categoryId,
            brandId
        );

        // Raise domain event AFTER persistence → Id is guaranteed
        product.AddDomainEvent(new ProductCreatedEvent(product.Id, product.Name));

        return product;
    }
    
    public Result<Updated> UpdateDetails(string name, string description, Money price,
        string sku, int categoryId, int brandId)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ProductErrors.NameRequired;

        if (string.IsNullOrWhiteSpace(description))
            return ProductErrors.DescriptionRequired;

        if (price.Amount <= 0)
            return ProductErrors.PriceGreaterThanZero;

        if (string.IsNullOrWhiteSpace(sku))
            return ProductErrors.SkuRequired;

        if (categoryId <= 0)
            return ProductErrors.CategoryRequired;

        if (brandId <= 0)
            return ProductErrors.BrandRequired;

        Name = name.Trim();
        Description = description.Trim();
        Price = price;
        Sku = sku.ToUpper().Trim();
        CategoryId = categoryId;
        BrandId = brandId;

        return Result.Updated;
    }

    #endregion

    

    #region Business Methods
    public Result<Success> AddImage(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return ProductErrors.ImageUrlRequired;

        // Enforce unique URLs
        if (_images.Any(img => img.Url.Equals(url, StringComparison.OrdinalIgnoreCase)))
            return ProductErrors.DuplicateImage;

        bool isFirstImage = !_images.Any();
        var newImage = new ProductImage(url, Id, isFirstImage);
        _images.Add(newImage);

        return Result.Success;
    }
    
    public Result<Success> RemoveImage(int productImageId)
    {
        var imageToRemove = _images.FirstOrDefault(img => img.Id == productImageId);
        if (imageToRemove is null)
            return ProductErrors.ImageNotFount;

        bool wasPrimary = imageToRemove.IsPrimary;
        _images.Remove(imageToRemove);

        // If the removed image was primary, set a new primary if any images remain
        if (wasPrimary && _images.Any())
        {
            _images[0].SetAsPrimary();
        }

        return Result.Success;
    }

    public Result<Success> SetPrimaryImage(int productImageId)
    {
        var newPrimaryImage = _images.FirstOrDefault(img => img.Id == productImageId);
        if (newPrimaryImage is null)
            return ProductErrors.ImageNotFount;

        var currentPrimaryImage = _images.FirstOrDefault(img => img.IsPrimary);
        currentPrimaryImage?.UnsetAsPrimary();

        newPrimaryImage.SetAsPrimary();

        return Result.Success;
    }
    public Result<Updated> UpdatePrice(Money newPrice)
    {
        if (newPrice.Amount <= 0)
            return ProductErrors.PriceGreaterThanZero;

        if (Price != newPrice)
        {
            var oldPrice = Price;
            Price = newPrice;
            AddDomainEvent(new ProductPriceChangedEvent(Id, newPrice, oldPrice));
        }

        return Result.Updated;
    }

    public Result<Updated> UpdateStock(int quantity)
    {
        if (quantity < 0)
            return ProductErrors.StockQuantityGreaterThanZero;

        var oldStock = StockQuantity;
        StockQuantity = quantity;

        AddDomainEvent(new ProductStockChangedEvent(Id, oldStock, quantity));

        if (quantity <= 10 && quantity > 0)
            AddDomainEvent(new ProductLowStockEvent(Id, quantity));

        if (quantity == 0)
            AddDomainEvent(new ProductOutOfStockEvent(Id));

        return Result.Updated;
    }

    public Result<Updated> AdjustStock(int adjustment)
    {
        var newQuantity = StockQuantity + adjustment;
        if (newQuantity < 0)
            return ProductErrors.InsufficientStock;

        return UpdateStock(newQuantity);
    }
    public Result<Success> SetSpecification(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
            return ProductErrors.SpecificationKeyRequired;

        _specifications[key] = value;
        return Result.Success;
    }

    public Result<Success> RemoveSpecification(string key)
    {
        _specifications.Remove(key);
        return Result.Success;
    }
    
    public Result<Success> RemoveAllSpecification()
    {
        _specifications.Clear();
        return Result.Success;
    }

    public void MarkAsFeatured() => IsFeatured = true;
    public void UnmarkAsFeatured() => IsFeatured = false;
    
    public Result<Updated> Activate()
    {
        if (IsActive) return Result.Updated;

        IsActive = true;
        AddDomainEvent(new ProductActivatedEvent(Id));

        return Result.Updated;
    }

    public Result<Updated> Deactivate()
    {
        if (!IsActive) return Result.Updated;

        IsActive = false;
        AddDomainEvent(new ProductDeactivatedEvent(Id));

        return Result.Updated;
    }
    #endregion

    
    
    // Business rules
    public bool IsInStock() => IsActive && StockQuantity > 0;
    public bool IsLowStock() => IsActive && StockQuantity is > 0 and <= 10;
    
    
}
