using AutoMapper;
using ElectronicsShop.Application.Common.Extensions;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Queries.GetProducts;

public sealed class GetFeaturedProductsQueryHandler 
    : ResponseHandler, IRequestHandler<GetFeaturedProductsQuery, GenericResponse<List<ProductListResponse>>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetFeaturedProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<GenericResponse<List<ProductListResponse>>> Handle(
        GetFeaturedProductsQuery request,
        CancellationToken cancellationToken)
    {
        var featuredProducts = await _productRepository.GetFeaturedProducts(cancellationToken);
        
        var responses = _mapper.Map<List<ProductListResponse>>(featuredProducts);

        return Success(responses, "Featured products retrieved successfully");
    }
}