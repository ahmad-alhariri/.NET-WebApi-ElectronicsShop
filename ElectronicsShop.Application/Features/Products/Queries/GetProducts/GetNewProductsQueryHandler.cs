using AutoMapper;
using ElectronicsShop.Application.Common.Extensions;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Queries.GetProducts;

public sealed class GetNewProductsQueryHandler 
    : ResponseHandler, IRequestHandler<GetNewProductsQuery, GenericResponse<List<ProductResponse>>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetNewProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<GenericResponse<List<ProductResponse>>> Handle(
        GetNewProductsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _productRepository
            .GetAllAsNoTracking()
            .AsQueryable()
            .Where(p => p.IsActive && p.CreatedDate >= DateTime.UtcNow.AddMonths(-1)) 
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
            "New products retrieved successfully"
        );
    }
}