using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Domain.Products;

namespace ElectronicsShop.Application.Interfaces.Repositories;

public interface IProductRepository:IGenericRepository<Product>
{
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<Product?> GetByIdWithIncludesAsync(int productId);
    Task<Product?> GetProductByIdWithImages(int id);
    Task<IEnumerable<Product>?> GetFeaturedProducts(CancellationToken cancellationToken);
    IQueryable<Product?> GetLowStockProducts(int threshold);
    IQueryable<Product?> GetNewProducts();
    Task<IReadOnlyList<ProductSearchDto>?> SearchProducts(string term, int maxResults, CancellationToken cancellationToken);
    
}