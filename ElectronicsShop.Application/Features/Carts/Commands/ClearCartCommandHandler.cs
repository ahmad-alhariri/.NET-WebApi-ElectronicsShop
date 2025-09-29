using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Carts;
using ElectronicsShop.Domain.Common.Results;
using MediatR;

namespace ElectronicsShop.Application.Features.Carts.Commands;

public class ClearCartCommandHandler:ResponseHandler, IRequestHandler<ClearCartCommand, GenericResponse<Unit>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ClearCartCommandHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService
    )
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }


    public async Task<GenericResponse<Unit>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        // 1. Get and check the appropriate cart
        var cart = await ResolveCartAsync(cancellationToken);
        
        if (cart == null)
        {
            return NotFound<Unit>("Cart not found");
        }

        // 2. Clear the cart
        cart.Clear();

        // 3. Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Success(Unit.Value, "Cart cleared successfully");
    }
    
    private async Task<Cart?> ResolveCartAsync(CancellationToken cancellationToken)
    {
        var (userId, anonymousId) = await _currentUserService.GetIdentifiers();

        Cart? cart = null;
        // For authenticated users, use or create their cart
        if (userId.HasValue)
        {
            cart = await _cartRepository.GetCartByUserIdAsync(userId.Value, cancellationToken);
        }
        // For anonymous users, use existing or create new cart
        else if (anonymousId.HasValue)
        {
            cart = await _cartRepository.GetCartByAnonymousIdAsync(anonymousId.Value, cancellationToken);
        }

        return cart;
    }
}