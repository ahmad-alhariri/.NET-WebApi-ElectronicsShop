using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Queries.GetProducts;

public sealed class GetProductStatisticsQueryHandler
    :ResponseHandler, IRequestHandler<GetProductStatisticsQuery, GenericResponse<ProductStatisticsResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetProductStatisticsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<GenericResponse<ProductStatisticsResponse>> Handle(
        GetProductStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        
        var total = await _productRepository.CountAsync(cancellationToken:cancellationToken);
        var active = await _productRepository.CountAsync(p => p.IsActive,cancellationToken);
        var inactive = total - active;
        var lowStock = await _productRepository.CountAsync(p => p.StockQuantity > 0 && p.StockQuantity <= 10, cancellationToken);
        var outOfStock = await _productRepository.CountAsync(p => p.StockQuantity == 0, cancellationToken);

        var stats = new ProductStatisticsResponse(total, active, inactive, lowStock, outOfStock);

        return Success(stats, "Product statistics retrieved successfully");
    }
}