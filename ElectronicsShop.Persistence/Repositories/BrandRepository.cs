using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Domain.Products.Brands;
using ElectronicsShop.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Persistence.Repositories;

public class BrandRepository : GenericRepository<Brand>, IBrandRepository
{
    private readonly DbSet<Brand> _brands;

    public BrandRepository(ApplicationDbContext context) : base(context)
    {
        _brands = context.Set<Brand>();
    }

    public async Task<Brand?> GetBrandWithProductsAsync(int brandId)
    {
        return await _brands.AsNoTracking()
            .Include(b => b.Products)
            .FirstOrDefaultAsync(b => b.Id == brandId);
    }
}