using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.DeleteProduct;

public sealed class RemoveSpecificationCommandHandler: ResponseHandler ,IRequestHandler<RemoveSpecificationCommand, GenericResponse<Unit>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveSpecificationCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GenericResponse<Unit>> Handle(RemoveSpecificationCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
            return NotFound<Unit>($"Product with Id {request.ProductId} not found.");

        var result = product.RemoveSpecification(request.SpecificationKey);
        if (result.IsError)
            return BadRequest<Unit>(result.Errors.First().Description);

        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success(Unit.Value, "Specification removed successfully.");
    }
}