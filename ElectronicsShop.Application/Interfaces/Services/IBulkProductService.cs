using ElectronicsShop.Application.Features.Products.Dtos;

namespace ElectronicsShop.Application.Interfaces.Services;

public interface IBulkProductService
{
    byte[] ExportProducts(IEnumerable<ProductExportDto> products);
    
    Task<IEnumerable<ProductImportResult>> ImportProductsAsync(
        Stream fileStream, 
        string contentType,
        CancellationToken cancellationToken = default);
}