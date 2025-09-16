using ElectronicsShop.Domain.Products.Categories;

namespace ElectronicsShop.Application.Interfaces.Repositories;

public interface ICategoryRepository:IGenericRepository<Category>
{
    Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

}