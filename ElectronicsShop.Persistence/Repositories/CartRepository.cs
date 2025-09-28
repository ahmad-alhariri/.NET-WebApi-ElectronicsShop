using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Domain.Carts;
using ElectronicsShop.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Persistence.Repositories;

public class CartRepository:GenericRepository<Cart>, ICartRepository
{
    private readonly DbSet<Cart> _carts;
    public CartRepository(ApplicationDbContext context) : base(context)
    {
        _carts = context.Set<Cart>();
    }

    public async Task<Cart?> GetCartByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _carts
            .Include(sc => sc.Items)
            .ThenInclude(ci => ci.Product)
            .ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(sc => sc.UserId == userId, cancellationToken);
    }

    public async Task<Cart?> GetCartByAnonymousIdAsync(Guid anonymousId, CancellationToken cancellationToken)
    {
        return await _carts
            .Include(c => c.Items)
            .ThenInclude(ci => ci.Product)
            .ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(c => c.Id == anonymousId && c.UserId == null, cancellationToken);
    }
}