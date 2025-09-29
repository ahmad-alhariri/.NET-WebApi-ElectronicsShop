using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Carts;
using ElectronicsShop.Domain.Common.Results;

namespace ElectronicsShop.Infrastructure.Services;

public class CartMigrationService: ICartMigrationService
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CartMigrationService(ICartRepository cartRepository, IProductRepository productRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }
    public async Task<Result<Success>> MigrateAnonymousCartToUserAsync(Guid anonymousId, Guid userId,CancellationToken cancellationToken)
    {
        
        var userCart = await _cartRepository.GetCartByUserIdAsync(userId,cancellationToken);
        var anonymousCart = await _cartRepository.GetCartByAnonymousIdAsync(anonymousId,cancellationToken);
        
        // 2. If no anonymous cart, nothing to migrate
        if (anonymousCart == null || anonymousCart.IsEmpty)
        {
            _currentUserService.RemoveAnonymousId();
            return Result.Success;
        }
        
        // 3. If user has no existing cart, simply reassign the anonymous cart
        if (userCart == null)
        {
            anonymousCart.AssignToUser(userId);
            _cartRepository.Update(anonymousCart);
        }
        // 4. If user has an existing cart, merge items
        else
        {
            foreach (var anonymousItem in anonymousCart.Items.ToList())
            {
                userCart.MergeWithAddItem(anonymousItem!.Product, anonymousItem.Quantity);
            }
            
            _cartRepository.Update(userCart);
            _cartRepository.Remove(anonymousCart);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _currentUserService.RemoveAnonymousId();
        return Result.Success;
    }
        
}