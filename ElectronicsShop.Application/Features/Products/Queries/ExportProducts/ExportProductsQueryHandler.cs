using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Application.Features.Products.Queries.ExportProducts;

public sealed class ExportProductsQueryHandler 
    : IRequestHandler<ExportProductsQuery, byte[]>
{
    private readonly IProductRepository _productRepository;
    private readonly IBulkProductService _exportService;

    public ExportProductsQueryHandler(
        IProductRepository productRepository,
        IBulkProductService exportService)
    {
        _productRepository = productRepository;
        _exportService = exportService;
    }

    public async Task<byte[]> Handle(
        ExportProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await _productRepository
            .GetAll()
            .Select(p => new ProductExportDto
            {
                Name = p.Name,
                Description = p.Description,
                Price = p.Price.Amount,
                Currency = p.Price.Currency,
                StockQuantity = p.StockQuantity,
                Sku = p.Sku,
                CategoryId = p.CategoryId,
                Category = p.Category!.Name,
                BrandId = p.BrandId,
                Brand = p.Brand!.Name,
                IsActive = p.IsActive,
                IsFeatured = p.IsFeatured
            })
            .ToListAsync(cancellationToken);

        return _exportService.ExportProducts(products);
    }
}