using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Common.Settings;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Domain.Common.Results;
using ElectronicsShop.Domain.Common.ValueObjects;
using MediatR;
using Microsoft.Extensions.Options;

namespace ElectronicsShop.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler:ResponseHandler,IRequestHandler<UpdateProductCommand,GenericResponse<int>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBrandRepository _brandRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly CurrencySettings _currencySettings;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IBrandRepository brandRepository,
        ICategoryRepository categoryRepository,
        IOptions<CurrencySettings> currencySettings)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _brandRepository = brandRepository;
        _categoryRepository = categoryRepository;
        _currencySettings = currencySettings.Value;
    }
    
    public async Task<GenericResponse<int>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        // Fetch the existing product aggregate from the database
        var product = await _productRepository.GetByIdAsync(request.Id);

        if (product is null)
        {
            return NotFound<int>($"Product with ID {request.Id} not found.");
        }
        
        // Validate related entities (Category and Brand)
        var categoryExists = await _categoryRepository.ExistsAsync(c => c.Id == request.CategoryId,cancellationToken);
        if (!categoryExists)
        {
            return Conflict<int>("Invalid category ID.");
        }

        var brandExists = await _brandRepository.ExistsAsync(b => b.Id == request.BrandId,cancellationToken);
        if (!brandExists)
        {
            return Conflict<int>("Invalid brand ID.");
        }
        
        // Check if the SKU is unique (excluding the current product)
        var isSkuUniq = await _productRepository.ExistsAsync(p => p.Sku == request.Sku && p.Id != request.Id, cancellationToken);
        if (isSkuUniq)
        {
            return Conflict<int>("SKU must be unique.");
        }
        
        // Create the Money value object for the price
        var price = new Money(request.PriceAmount, _currencySettings.DefaultCurrency);
        

        // Call the domain entity's method to perform the update
        var updateResult = product.UpdateDetails(
            request.Name,
            request.Description,
            price,
            request.Sku,
            request.CategoryId,
            request.BrandId
        );

        if (updateResult.IsError)
        {
            // Propagate the domain-level error
            return Conflict<int>(updateResult.Errors.First().Description);
        }
        
        // 5. Update specifications
        if (request.Specifications is not null)
        {
            // First, clear existing specifications
            product.RemoveAllSpecification();
            
            // Then, add the new ones
            foreach (var spec in request.Specifications)
            {
                product.SetSpecification(spec.Key, spec.Value);
            }
        }

        // 6. Persist the changes to the database
        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Success(product.Id, "Product updated successfully.");

    }
}