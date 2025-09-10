using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Common.ValueObjects;
using ElectronicsShop.Domain.Products;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler: ResponseHandler, IRequestHandler<CreateProductCommand,GenericResponse<int>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IFileService fileService,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
    }
    
    public async Task<GenericResponse<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // check on category and brand existence 
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (category is null)
            return NotFound<int>("Invalid category ID. The specified category does not exist.");

        var brand = await _brandRepository.GetByIdAsync(request.BrandId);
        if (brand is null)
            return NotFound<int>("Invalid Brand ID. The specified Brand does not exist.");
        
        // Convert to Money value object
        var price = new Money(request.PriceAmount, request.PriceCurrency);

        // Create product via factory method
        var productResult = Product.Create(
            request.Name,
            request.Description,
            price,
            request.StockQuantity,
            request.Sku,
            request.CategoryId,
            request.BrandId
        );

        if (productResult.IsError)
            return BadRequest<int>(productResult.Errors.First().Description);

        var product = productResult.Value;

        // 4. Handle Specifications
        if (request.Specifications is not null)
        {
            foreach (var spec in request.Specifications)
            {
                // Let the domain entity handle the logic
                product.SetSpecification(spec.Key, spec.Value);

            }
        }

        // Save images if provided
        if (request.Images is not null)
        {
            foreach (var image in request.Images)
            {
                var imageUrl = await _fileService.SaveImageAsync(image, "Products");
                var result = product.AddImage(imageUrl);
                if (result.IsError)
                {
                    return UnprocessableEntity<int>(result.Errors.First().Description);
                }
            }
        }

        // Persist
        await _productRepository.AddAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success(product.Id, "Product created successfully");
    }
}