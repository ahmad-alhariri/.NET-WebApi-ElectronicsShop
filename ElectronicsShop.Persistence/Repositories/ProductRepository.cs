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
    
}