using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Carts;
using ElectronicsShop.Domain.Common.Results;
using ElectronicsShop.Domain.Products;
using MediatR;

namespace ElectronicsShop.Application.Features.Carts.Commands;

public class AddItemToCartCommandHandler : ResponseHandler, IRequestHandler<AddItemToCartCommand, GenericResponse<Unit>>
{
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public AddItemToCartCommandHandler(IProductRepository productRepository, ICartRepository cartRepository, 
        IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _productRepository = productRepository;
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<GenericResponse<Unit>> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate the product exists and has sufficient stock
        var validationResult = await ValidateProductAsync(request.ProductId, request.Quantity);
        if (validationResult.IsError)
            return BadRequest<Unit>(validationResult.Errors.First().Description);
        
        var product = validationResult.Value;

        // 2. Get or create the appropriate cart
        var cart = await ResolveCartAsync(cancellationToken);

        // 3. Add item to cart
        var addItemResult = cart.AddItem(product, request.Quantity);
        if (addItemResult.IsError)
            return Conflict<Unit>(addItemResult.Errors.First().Description);

        // 4. Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Success(Unit.Value, "Item added to cart successfully");
    }

    private async Task<Cart> ResolveCartAsync(CancellationToken cancellationToken)
    {
        var (userId, anonymousId) = await _currentUserService.GetIdentifiers();

        // For authenticated users, use or create their cart
        if (userId.HasValue)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId.Value, cancellationToken);
            if (cart == null)
            {
                cart = Cart.Create(userId.Value);
                await _cartRepository.AddAsync(cart, cancellationToken);
            }
            return cart;
        }

        // For anonymous users, use existing or create new cart
        if (anonymousId.HasValue)
        {
            var cart = await _cartRepository.GetCartByAnonymousIdAsync(anonymousId.Value, cancellationToken);
            if (cart != null)
                return cart;
        }

        // Create new anonymous cart
        var newCart = Cart.Create();
        await _cartRepository.AddAsync(newCart, cancellationToken);
        _currentUserService.AppendAnonymousId(newCart.Id);
        return newCart;
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
}