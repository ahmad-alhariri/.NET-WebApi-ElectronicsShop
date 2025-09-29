using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Carts;
using ElectronicsShop.Domain.Common.Results;
using ElectronicsShop.Domain.Products;
using MediatR;

namespace ElectronicsShop.Application.Features.Carts.Commands;

public class UpdateCartItemCommandHandler:ResponseHandler, IRequestHandler<UpdateCartItemCommand,GenericResponse<Unit>>
{
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartRepository _cartRepository;

    public UpdateCartItemCommandHandler(IProductRepository productRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork, ICartRepository cartRepository)
    {
        _productRepository = productRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
        _cartRepository = cartRepository;
    }
    
    public async Task<GenericResponse<Unit>> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate the product exists and has sufficient stock
        var validationResult = await ValidateProductAsync(request.ProductId, request.Quantity);
        if (validationResult.IsError)
            return BadRequest<Unit>(validationResult.Errors.First().Description);
        
        var product = validationResult.Value;

        // 2. Get and check the appropriate cart
        var cart = await ResolveCartAsync(cancellationToken);
        if (cart == null)
        {
            return NotFound<Unit>("Cart not found");
        }

        // 3. Update item in cart
        var updateItemResult = cart.UpdateItemQuantity(product, request.Quantity);
        if (updateItemResult.IsError)
            return Conflict<Unit>(updateItemResult.Errors.First().Description);

        // 4. Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Success(Unit.Value, "Cart Updated successfully");
    }
    
    private async Task<Result<Product>> ValidateProductAsync(int productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
            return Error.NotFound("","Product not found.");

        if (quantity > product.StockQuantity)
            return Error.Validation("",
                $"Insufficient stock for '{product.Name}'. Only {product.StockQuantity} units available.");

        return product;
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