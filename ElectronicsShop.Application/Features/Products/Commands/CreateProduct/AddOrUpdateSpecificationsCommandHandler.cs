using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.CreateProduct;

public class AddOrUpdateSpecificationsCommandHandler:ResponseHandler, IRequestHandler<AddOrUpdateSpecificationsCommand, GenericResponse<Unit>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddOrUpdateSpecificationsCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<GenericResponse<Unit>> Handle(AddOrUpdateSpecificationsCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
            return NotFound<Unit>($"Product with Id {request.ProductId} not found.");

        
        if (request.Specifications is not null)
        {
            foreach (var spec in request.Specifications)
            {
                // Let the domain entity handle the logic
                var result = product.SetSpecification(spec.Key, spec.Value);
                if (result.IsError)
                {
                    return BadRequest<Unit>("Failed to add/update specifications: " + result.Errors.First().Description);
                }

            }
        }

        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success(Unit.Value, "Specifications added/updated successfully.");
    }
}