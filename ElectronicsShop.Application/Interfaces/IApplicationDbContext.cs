using ElectronicsShop.Domain.Products;
using ElectronicsShop.Domain.Products.Brands;
using ElectronicsShop.Domain.Products.Categories;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Application.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Category> Categories { get; }
    public DbSet<Brand> Brands { get; }
    public DbSet<Product> Products { get; }
    public DbSet<ProductImage> ProductImages { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}