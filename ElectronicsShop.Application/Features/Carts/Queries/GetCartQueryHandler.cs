using AutoMapper;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Carts.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using MediatR;

namespace ElectronicsShop.Application.Features.Carts.Queries;

public class GetCartQueryHandler:ResponseHandler,IRequestHandler<GetCartQuery,GenericResponse<CartResponse>>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetCartQueryHandler(ICartRepository cartRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }
    
    public async Task<GenericResponse<CartResponse>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var (userId, anonymousId) = await _currentUserService.GetIdentifiers();

        // Retrieve the cart based on userId or anonymousId
        var cart = userId.HasValue
            ? await _cartRepository.GetCartByUserIdAsync(userId.Value, cancellationToken)
            : anonymousId.HasValue
                ? await _cartRepository.GetCartByAnonymousIdAsync(anonymousId.Value, cancellationToken)
                : null;

        if (cart == null)
        {
            // Return an empty cart response
            return Success(new CartResponse(), "Cart is empty.");
        }

        // Use AutoMapper to map the cart to CartResponse (including items, subtotal, total items)
        var cartResponse = _mapper.Map<CartResponse>(cart);
        return Success(cartResponse, "Cart retrieved successfully.");
    }
}