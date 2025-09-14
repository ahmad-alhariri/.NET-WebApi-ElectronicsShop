using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Domain.Products;
using ElectronicsShop.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Persistence.Repositories;

public class ProductRepository: GenericRepository<Product>, IProductRepository
{
    private readonly DbSet<Product> _products;
    
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        _products = context.Set<Product>();
    }

    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        return await _products
            .FirstOrDefaultAsync(p => p.Sku == sku, cancellationToken);
    }

    public async Task<Product?> GetByIdWithIncludesAsync(int productId)
    {
        return await _products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Images)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<Product?> GetProductByIdWithImages(int id)
    {
        return await _products
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyList<ProductSearchDto>?> SearchProducts(string term, int maxResults, CancellationToken cancellationToken)
    {
        var products = await _products
            .AsNoTracking()
            .Where(p => p.IsActive && p.Name.ToLower().Contains(term))
            .OrderBy(p => p.Name) // or popularity, sales, etc.
            .Take(maxResults)
            .Select(p => new ProductSearchDto(p.Id, p.Name))
            .ToListAsync(cancellationToken);

        return products;
    }
}