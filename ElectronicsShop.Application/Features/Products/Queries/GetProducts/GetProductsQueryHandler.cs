using AutoMapper;
using ElectronicsShop.Application.Common.Extensions;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Brands.Dtos;
using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler:ResponseHandler, IRequestHandler<GetProductsQuery, GenericResponse<List<ProductListResponse>>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    
    public async Task<GenericResponse<List<ProductListResponse>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var productsQuery =  _productRepository.GetAll()
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Images)
            .AsNoTracking()
            .AsQueryable();
        
        // == FILTERING ==
        if (request.CategoryId.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.CategoryId == request.CategoryId.Value);
        }
        if (request.BrandId.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.BrandId == request.BrandId.Value);
        }
        if (request.MinPrice.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.Price.Amount >= request.MinPrice.Value);
        }
        if (request.MaxPrice.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.Price.Amount <= request.MaxPrice.Value);
        }
        
        if (request.IsOutOfStock.HasValue)
        {
            if (request.IsOutOfStock.Value)
            {
                productsQuery = productsQuery.Where(p => p.StockQuantity == 0);
            }
            else
            {
                productsQuery = productsQuery.Where(p => p.StockQuantity > 0);
            }
        }
        
        if (request.IsNew.HasValue)
        {
            if (request.IsNew.Value)
            {
                productsQuery = productsQuery.Where(p => p.CreatedDate >= DateTime.UtcNow.AddMonths(-1));
            }
        }
        
        if (request.IsFeatured.HasValue)
        {
            if (request.IsFeatured.Value)
            {
                productsQuery = productsQuery.Where(p => p.IsFeatured);
            }
        }
        
        // == SEARCHING ==
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTermLower = request.SearchTerm.ToLower();
            productsQuery = productsQuery.Where(p => 
                p.Name.ToLower().Contains(searchTermLower) || 
                p.Description.ToLower().Contains(searchTermLower) || 
                p.Sku.ToLower().Contains(searchTermLower));
        }
        
        var isDescending = request.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase);
        
        productsQuery = request.SortColumn.ToLower() switch
        {
            "createdat" => isDescending ? productsQuery.OrderByDescending(wo => wo.CreatedDate) : productsQuery.OrderBy(wo => wo.CreatedDate),
            "name" => isDescending ? productsQuery.OrderByDescending(wo => wo.Name) : productsQuery.OrderBy(wo => wo.Name),
            "sku" =>  isDescending ? productsQuery.OrderByDescending(wo => wo.Sku) : productsQuery.OrderBy(wo => wo.Sku),
            "price" =>  isDescending ? productsQuery.OrderByDescending(wo => wo.Price.Amount) : productsQuery.OrderBy(wo => wo.Price.Amount),
            _ => productsQuery.OrderByDescending(wo => wo.CreatedDate) // Default sorting
        };
        
        var pagedProducts = await productsQuery.ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);

        var brandResponses = _mapper.Map<List<ProductListResponse>>(pagedProducts.Items);
        
        return Paginated(brandResponses, pagedProducts.TotalCount, pagedProducts.PageNumber, pagedProducts.PageSize, "Brands retrieved successfully");
    }
}