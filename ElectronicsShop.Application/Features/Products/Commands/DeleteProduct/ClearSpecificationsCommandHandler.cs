using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.DeleteProduct;

public sealed class ClearSpecificationsCommandHandler 
    : ResponseHandler, IRequestHandler<ClearSpecificationsCommand, GenericResponse<Unit>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClearSpecificationsCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GenericResponse<Unit>> Handle(ClearSpecificationsCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
            return NotFound<Unit>($"Product with Id {request.ProductId} not found.");

        product.RemoveAllSpecification();

        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success(Unit.Value, "All specifications cleared successfully.");
    }
}