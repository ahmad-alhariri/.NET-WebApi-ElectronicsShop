using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Domain.Products;

namespace ElectronicsShop.Application.Features.Products.Mappers;

public partial class ProductProfile
{
    private void ProductListResponseMapper()
    {
        CreateMap<Product, ProductListResponse>()
            .ForMember(dest => dest.PriceAmount,
                opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.PriceCurrency,
                opt => opt.MapFrom(src => src.Price.Currency))
            .ForMember(dest => dest.IsInStock,
                opt => opt.MapFrom(src => src.StockQuantity > 0))
            .ForMember(dest => dest.IsNew,
                opt => opt.MapFrom(src => src.CreatedDate >= DateTime.UtcNow.AddMonths(-1)))
            
            // Map other navigation properties
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.BrandName,
                opt => opt.MapFrom(src => src.Brand.Name))
            .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.Images.FirstOrDefault(i => i.IsPrimary).Url));

    }
}