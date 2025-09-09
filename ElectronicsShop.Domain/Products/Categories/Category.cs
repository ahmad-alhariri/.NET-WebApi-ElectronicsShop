using ElectronicsShop.Domain.Common;
using ElectronicsShop.Domain.Common.Results;

namespace ElectronicsShop.Domain.Products.Categories;

public sealed class Category : BaseAuditableEntity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string ImageUrl { get; private set; }

    // Navigation property for EF Core
    private readonly List<Product> _products = [];
    public IEnumerable<Product> Products => _products.AsReadOnly();

    // Private constructor for EF Core
    private Category() { }

    private Category(string name, string? description, string imageUrl)
    {
        // Enforce invariants on creation
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name), "Category name cannot be empty.");
        }

        Name = name.Trim();
        Description = description?.Trim();
        ImageUrl = imageUrl.Trim();
    }
    
    public static Result<Category> Create(string name, string? description, string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return CategoryErrors.NameRequired;
        }
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return CategoryErrors.NameRequired;
        }

        return new Category(name.Trim(), description, imageUrl);
    }

    public Result<Updated> UpdateDetails(string newName, string? newDescription, string newImageUrl)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            return CategoryErrors.NameRequired;
        }

        Name = newName.Trim();
        Description = newDescription?.Trim();
        ImageUrl = newImageUrl?.Trim() ?? string.Empty;

        return Result.Updated;
    }
}