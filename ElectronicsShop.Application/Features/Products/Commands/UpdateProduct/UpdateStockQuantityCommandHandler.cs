using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.UpdateProduct;

public sealed class UpdateStockQuantityCommandHandler: ResponseHandler, IRequestHandler<UpdateStockQuantityCommand, GenericResponse<Unit>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStockQuantityCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GenericResponse<Unit>> Handle(UpdateStockQuantityCommand request, CancellationToken cancellationToken)
    {
        // 1. Load product
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
        {
            return NotFound<Unit>($"Product with Id {request.ProductId} not found.");
        }

        // 2. Apply business rule via domain method
        var result = product.UpdateStock(request.Quantity);
        if (result.IsError)
        {
            return BadRequest<Unit>(result.Errors.FirstOrDefault().Description);
        }

        // 3. Persist
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success(Unit.Value, "Stock quantity updated successfully.");
    }
}