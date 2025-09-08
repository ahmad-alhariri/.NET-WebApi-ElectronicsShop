using AutoMapper;
using ElectronicsShop.Application.Features.Brands.Dtos;
using ElectronicsShop.Domain.Products.Brands;

namespace ElectronicsShop.Application.Features.Brands.Mappers;

public partial class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<Brand, BrandResponse>();
    }
}