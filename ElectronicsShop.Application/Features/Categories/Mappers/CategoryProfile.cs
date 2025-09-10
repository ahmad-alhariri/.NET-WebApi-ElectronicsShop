using AutoMapper;
using ElectronicsShop.Application.Features.Categories.Dtos;
using ElectronicsShop.Domain.Products.Categories;

namespace ElectronicsShop.Application.Features.Categories.Mappers;

public class CategoryProfile:Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryResponse>()
            .ForMember(dest => dest.ProductCount,
                opt => opt.MapFrom(src => src.Products.Count()));
    }
    
}