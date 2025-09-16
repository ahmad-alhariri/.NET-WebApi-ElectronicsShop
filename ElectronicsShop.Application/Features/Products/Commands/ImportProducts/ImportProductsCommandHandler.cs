using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Application.Interfaces.Services;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.ImportProducts;

public class ImportProductsCommandHandler: IRequestHandler<ImportProductsCommand, IEnumerable<ProductImportResult>>
{
    private readonly IBulkProductService _bulkService;

    public ImportProductsCommandHandler(IBulkProductService bulkService)
    {
        _bulkService = bulkService;
    }

    public async Task<IEnumerable<ProductImportResult>> Handle(ImportProductsCommand request, CancellationToken cancellationToken)
    {
        await using var stream = request.File.OpenReadStream();
        return await _bulkService.ImportProductsAsync(stream, request.File.ContentType, cancellationToken);
    }
}