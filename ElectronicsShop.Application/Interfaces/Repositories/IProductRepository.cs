using ElectronicsShop.Domain.Products;

namespace ElectronicsShop.Application.Interfaces.Repositories;

public interface IProductRepository:IGenericRepository<Product>
{
    Task<Product?> GetByIdWithIncludesAsync(int productId);
    Task<Product?> GetProductByIdWithImages(int id);
    
}