using ElectronicsShop.Domain.Carts;

namespace ElectronicsShop.Application.Interfaces.Repositories;

public interface ICartRepository: IGenericRepository<Cart>
{
    Task<Cart?> GetCartByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<Cart?> GetCartByAnonymousIdAsync(Guid anonymousId, CancellationToken cancellationToken);
}