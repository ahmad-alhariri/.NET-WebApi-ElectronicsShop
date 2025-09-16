using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Domain.Products;
using ElectronicsShop.Domain.Products.Categories;
using ElectronicsShop.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Persistence.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    private readonly DbSet<Category> _categories;

    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
        _categories = context.Set<Category>();
    }


    public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _categories
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }
}