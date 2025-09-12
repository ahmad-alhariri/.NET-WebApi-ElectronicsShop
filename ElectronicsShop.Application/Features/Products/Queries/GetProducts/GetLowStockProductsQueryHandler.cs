using AutoMapper;
using ElectronicsShop.Application.Common.Extensions;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Queries.GetProducts;

public sealed class GetLowStockProductsQueryHandler 
    : ResponseHandler,IRequestHandler<GetLowStockProductsQuery, GenericResponse<List<ProductResponse>>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetLowStockProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<GenericResponse<List<ProductResponse>>> Handle(
        GetLowStockProductsQuery request, 
        CancellationToken cancellationToken)
    {
        var query = _productRepository
            .GetAllAsNoTracking().AsQueryable()
            .Where(p => p.IsActive && p.StockQuantity <= request.Threshold)
            .OrderBy(p => p.StockQuantity); 

        var pagedProducts = await query.ToPagedListAsync(
            request.Page, 
            request.PageSize, 
            cancellationToken);

        var productDtos = _mapper.Map<List<ProductResponse>>(pagedProducts.Items);

        return Paginated(
            productDtos,
            pagedProducts.TotalCount,
            pagedProducts.PageNumber,
            pagedProducts.PageSize,
            "Low stock products retrieved successfully"
        );
    }
}