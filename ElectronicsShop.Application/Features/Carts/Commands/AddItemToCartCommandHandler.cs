using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Carts;
using MediatR;

namespace ElectronicsShop.Application.Features.Carts.Commands;

public class AddItemToCartCommandHandler:ResponseHandler, IRequestHandler<AddItemToCartCommand,GenericResponse<Unit>>
{
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public AddItemToCartCommandHandler(IProductRepository productRepository,ICartRepository cartRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _productRepository = productRepository;
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }
    
    public async Task<GenericResponse<Unit>> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var (userId, anonymousId) = await _currentUserService.GetIdentifiers();
        
        // 1. Validate the product exists and has sufficient stock
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            return NotFound<Unit>("Product not found.");
        }
        if (request.Quantity > product.StockQuantity)
        {
            return Conflict<Unit>(
                $"Insufficient stock for '{product.Name}'. Only {product.StockQuantity} units available.");
        }
        
        Cart? cart;
        
        // A. Handle registered user
        if (userId.HasValue)
        {
            cart = await _cartRepository.GetCartByUserIdAsync(userId.Value,cancellationToken);
            if (cart == null)
            {
                cart = Cart.Create(userId.Value);
                await _cartRepository.AddAsync(cart,cancellationToken);
            }
            
        }
        // B. Handle anonymous user with existing cart
        else 
        {
            cart = await _cartRepository.GetCartByAnonymousIdAsync(anonymousId.Value, cancellationToken);
            if (cart is null)
            {
                cart = Cart.Create(null);
                await _cartRepository.AddAsync(cart,cancellationToken);
                anonymousId = cart.Id;
                _currentUserService.AppendAnonymousId(anonymousId);
            }
        }
        
        var result = cart.AddItem(product, request.Quantity);

        if (result.IsError)
        {
            return Conflict<Unit>(result.Errors.First().Description);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success(Unit.Value, "Item added to cart successfully");


    }
}