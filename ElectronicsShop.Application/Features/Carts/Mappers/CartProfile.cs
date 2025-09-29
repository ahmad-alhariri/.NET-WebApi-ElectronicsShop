using AutoMapper;
using ElectronicsShop.Application.Features.Carts.Dtos;
using ElectronicsShop.Domain.Carts;

namespace ElectronicsShop.Application.Features.Carts.Mappers;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<CartItem, CartItemResponse>()
            .ForMember(dest => dest.CartItemId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProductId,
                opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
            .ForMember(dest => dest.Price,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Price.Amount : 0))
            .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src => 
                    src.Product != null && 
                    src.Product.Images != null && 
                    src.Product.Images.Any() 
                        ? src.Product.Images.First().Url 
                        : string.Empty));

        CreateMap<Cart, CartResponse>()
            .ForMember(dest => dest.TotalItems,
                opt => opt.MapFrom(src => src.Items != null ? src.Items.Sum(ci => ci.Quantity) : 0))
            .ForMember(dest => dest.Subtotal,
                opt => opt.MapFrom(src => 
                    src.Items != null 
                        ? src.Items.Where(ci => ci.Product != null)
                            .Sum(ci => ci.Quantity * ci.Product!.Price.Amount) 
                        : 0));
    }
}