using AutoMapper;
using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Domain.Products;

namespace ElectronicsShop.Application.Features.Products.Mappers;

public partial class ProductProfile:Profile
{
    public ProductProfile()
    {
        ProductResponseMapper();
        ProductListResponseMapper();
        
    }

}