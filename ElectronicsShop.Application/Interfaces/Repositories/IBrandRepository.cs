using ElectronicsShop.Domain.Products.Brands;

namespace ElectronicsShop.Application.Interfaces.Repositories;

public interface IBrandRepository:IGenericRepository<Brand>
{
    Task<Brand?> GetBrandWithProductsAsync(int brandId);
}