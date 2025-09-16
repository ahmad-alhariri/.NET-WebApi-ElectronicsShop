using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.UpdateProduct.UpdatePrimaryImage;

public class SetPrimaryProductImageCommandHandler:ResponseHandler, IRequestHandler<SetPrimaryProductImageCommand, GenericResponse<Unit>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetPrimaryProductImageCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<GenericResponse<Unit>> Handle(SetPrimaryProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdWithImages(request.ProductId);
        if (product is null)
        {
            return NotFound<Unit>("Product not found");
        }
        var setResult = product.SetPrimaryImage(request.ImageId);
        if (setResult.IsError)
        {
            return Conflict<Unit>(setResult.Errors.FirstOrDefault().Description);
        }
        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Success(Unit.Value, "Primary image updated successfully");
    }
}