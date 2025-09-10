using AutoMapper;
using ElectronicsShop.Application.Features.Brands.Dtos;
using ElectronicsShop.Domain.Products.Brands;

namespace ElectronicsShop.Application.Features.Brands.Mappers;

public partial class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<Brand, BrandResponse>().ForMember(dest => dest.ProductCount,
            opt => opt.MapFrom(src => src.Products.Count()));;
    }
}