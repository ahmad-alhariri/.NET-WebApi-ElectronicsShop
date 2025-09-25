using System.Reflection;
using ElectronicsShop.Application.Interfaces;
using ElectronicsShop.Domain.Common;
using ElectronicsShop.Domain.Common.Interfaces;
using ElectronicsShop.Domain.Products;
using ElectronicsShop.Domain.Products.Brands;
using ElectronicsShop.Domain.Products.Categories;
using ElectronicsShop.Domain.Users;
using ElectronicsShop.Domain.Users.Address;
using ElectronicsShop.Domain.Users.Identity;
using ElectronicsShop.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Persistence.DataContext;


public class ApplicationDbContext : 
    IdentityDbContext<User, Role, Guid,
        IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>,
    IApplicationDbContext
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
    public DbSet<UserAddress> UserAddresses => Set<UserAddress>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    

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
 
        var entitiesWithEvents = ChangeTracker.Entries<IHasDomainEvents>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

 
        // After a successful save, dispatch domain events.
        await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);
 
        return result;
    }


}