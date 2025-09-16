using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Domain.Common.Results;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.UpdateProduct;

public class SetProductFeaturedStatusCommandHandler:ResponseHandler, IRequestHandler<SetProductFeaturedStatusCommand,GenericResponse<Unit>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetProductFeaturedStatusCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    
    
    public async Task<GenericResponse<Unit>> Handle(SetProductFeaturedStatusCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
        {
            return NotFound<Unit>("Product not found");
        }
        
        if (request.IsFeatured)
        {
            product.MarkAsFeatured();
        }
        else
        {
            product.UnmarkAsFeatured();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Success(Unit.Value, "Product featured status updated successfully.");

    }
}