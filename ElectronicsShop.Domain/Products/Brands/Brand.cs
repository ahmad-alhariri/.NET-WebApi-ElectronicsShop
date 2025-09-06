using ElectronicsShop.Domain.Common;
using ElectronicsShop.Domain.Common.Results;

namespace ElectronicsShop.Domain.Products.Brands;

public sealed class Brand : BaseAuditableEntity
{
    public string Name { get; private set; }
    public string? LogoUrl { get; private set; }

    // Navigation properties
    private readonly List<Product> _products = [];
    public IEnumerable<Product> Products => _products.AsReadOnly();

    private Brand()
    {
    }

    public Brand(string name, string? logoUrl)
    {
        Name = name.Trim();
        LogoUrl = logoUrl?.Trim();
    }
    
    public static Result<Brand> Create(string name, string? logoUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BrandErrors.NameRequired;

        return new Brand(name.Trim(), logoUrl?.Trim() ?? string.Empty);

    }

    public Result<Updated> UpdateDetails(string name, string? logoUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BrandErrors.NameRequired;

        Name = name.Trim();
        LogoUrl = logoUrl?.Trim() ?? string.Empty;

        return Result.Updated;
    }
    
}