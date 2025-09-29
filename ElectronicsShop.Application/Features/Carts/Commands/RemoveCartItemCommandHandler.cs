using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Carts;
using ElectronicsShop.Domain.Common.Results;
using MediatR;

namespace ElectronicsShop.Application.Features.Carts.Commands;

public class RemoveCartItemCommandHandler:ResponseHandler, IRequestHandler<RemoveCartItemCommand, GenericResponse<Unit>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public RemoveCartItemCommandHandler(
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
    
    public async Task<GenericResponse<Unit>> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate the product exists
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            return NotFound<Unit>("Product not found");
        
        // 2. Get and check the appropriate cart
        var cart = await ResolveCartAsync(cancellationToken);
        if (cart == null)
        {
            return NotFound<Unit>("Cart not found");
        }

        // 3. Remove item from cart
        var removeItemResult = cart.RemoveItem(request.ProductId);
        if (removeItemResult.IsError)
            return Conflict<Unit>(removeItemResult.Errors.First().Description);

        // 4. Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Success(Unit.Value, "Item removed from cart successfully");
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