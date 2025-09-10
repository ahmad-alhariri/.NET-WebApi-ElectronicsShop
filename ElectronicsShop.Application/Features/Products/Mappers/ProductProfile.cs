using AutoMapper;
using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Domain.Products;

namespace ElectronicsShop.Application.Features.Products.Mappers;

public partial class ProductProfile:Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductResponse>()
            // This rule tells AutoMapper how to get the PriceAmount
            .ForMember(dest => dest.PriceAmount,
                opt => opt.MapFrom(src => src.Price.Amount))
            
            // This rule tells AutoMapper how to get the PriceCurrency
            .ForMember(dest => dest.PriceCurrency, 
                opt => opt.MapFrom(src => src.Price.Currency))
            
            // Map other navigation properties
            .ForMember(dest => dest.CategoryName, 
                opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.BrandName, 
                opt => opt.MapFrom(src => src.Brand.Name))
            .ForMember(dest => dest.Specifications, 
                opt => opt.MapFrom(src => src.Specifications.Select(s => new SpecificationDto(s.Key, s.Value)).ToList()))
            .ForMember(dest => dest.ImageUrls, 
                opt =>  opt.MapFrom(src => src.Images.Select(i => i.Url)));
        
    }

}