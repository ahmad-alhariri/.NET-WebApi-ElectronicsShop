using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Queries.GetProducts;

public class SearchProductsQueryHandler:ResponseHandler, IRequestHandler<SearchProductsQuery, GenericResponse<IReadOnlyList<ProductSearchDto>>>
{
    private readonly IProductRepository _productRepository;

    public SearchProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<GenericResponse<IReadOnlyList<ProductSearchDto>>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Term))
        {
            return Success<IReadOnlyList<ProductSearchDto>>(Array.Empty<ProductSearchDto>());
        }
        
        var normalizedTerm = request.Term.Trim().ToLower();

        var products = await _productRepository.SearchProducts(normalizedTerm, request.MaxResults, cancellationToken);
         
        return Success(products ?? Array.Empty<ProductSearchDto>(), "Products retrieved successfully.");
    }
}