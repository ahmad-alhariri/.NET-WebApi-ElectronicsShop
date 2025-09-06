using System.Reflection;
using ElectronicsShop.Application.Interfaces;
using ElectronicsShop.Domain.Common;
using ElectronicsShop.Domain.Common.Interfaces;
using ElectronicsShop.Domain.Products;
using ElectronicsShop.Domain.Products.Brands;
using ElectronicsShop.Domain.Products.Categories;
using ElectronicsShop.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Persistence.DataContext;


public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IDomainEventDispatcher _dispatcher;
    
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDomainEventDispatcher dispatcher
        )
        : base(options)
    {
        _dispatcher = dispatcher;
    }
    
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Scans the current assembly for all IEntityTypeConfiguration classes and applies them.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
        
    }
    
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        // This call to the base method will trigger the AuditableEntitySaveChangesInterceptor.
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
         
        if (_dispatcher == null) return result;
 
        var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();
 
        // After a successful save, dispatch domain events.
        await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);
 
        return result;
    }


}