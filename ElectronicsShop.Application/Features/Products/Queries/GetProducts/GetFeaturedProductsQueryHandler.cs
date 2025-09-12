using AutoMapper;
using ElectronicsShop.Application.Common.Extensions;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Queries.GetProducts;

public sealed class GetFeaturedProductsQueryHandler 
    : ResponseHandler, IRequestHandler<GetFeaturedProductsQuery, GenericResponse<List<ProductResponse>>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetFeaturedProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<GenericResponse<List<ProductResponse>>> Handle(
        GetFeaturedProductsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _productRepository
            .GetAllAsNoTracking()
            .AsQueryable()
            .Where(p => p.IsActive && p.IsFeatured)
            .OrderByDescending(p => p.CreatedDate); 

        var pagedProducts = await query.ToPagedListAsync(
            request.Page,
            request.PageSize,
            cancellationToken);

        var dto = _mapper.Map<List<ProductResponse>>(pagedProducts.Items);

        return Paginated(
            dto,
            pagedProducts.TotalCount,
            pagedProducts.PageNumber,
            pagedProducts.PageSize,
            "Featured products retrieved successfully"
        );
    }
}