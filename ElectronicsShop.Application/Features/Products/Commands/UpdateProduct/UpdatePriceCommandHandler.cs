using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Domain.Common.ValueObjects;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.UpdateProduct;

public class UpdatePriceCommandHandler:ResponseHandler, IRequestHandler<UpdatePriceCommand, GenericResponse<Unit>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdatePriceCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GenericResponse<Unit>> Handle(UpdatePriceCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
            return NotFound<Unit>($"Product with Id {request.ProductId} not found.");

        var money = new Money(request.Amount, request.Currency);

        var result = product.UpdatePrice(money);
        if (result.IsError)
            return BadRequest<Unit>(result.Errors.First().Description);

        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success(Unit.Value, "Price updated successfully.");
    }
}