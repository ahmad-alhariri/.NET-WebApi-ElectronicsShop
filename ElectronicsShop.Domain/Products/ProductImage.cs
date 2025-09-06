using ElectronicsShop.Domain.Common;

namespace ElectronicsShop.Domain.Products;

public class ProductImage:BaseAuditableEntity
{
    public string Url { get; private set; }
    public bool IsPrimary { get; private set; }

    public int ProductId { get; private set; }

    private ProductImage() { } // For EF Core

    public ProductImage(string url, int productId, bool isPrimary = false)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentNullException(nameof(url));
        }

        Url = url;
        ProductId = productId;
        IsPrimary = isPrimary;
    }

    // Internal methods to be called ONLY by the Product aggregate
    internal void SetAsPrimary() => IsPrimary = true;
    internal void UnsetAsPrimary() => IsPrimary = false;
}