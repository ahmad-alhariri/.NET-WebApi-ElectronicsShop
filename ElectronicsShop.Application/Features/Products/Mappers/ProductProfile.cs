using AutoMapper;
using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Domain.Products;

namespace ElectronicsShop.Application.Features.Products.Mappers;

public class ProductProfile:Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name))
            .ForMember(dest => dest.ImagesUrl, opt => opt.MapFrom(src => src.Images.Select(i => i.Url)));
    }

}