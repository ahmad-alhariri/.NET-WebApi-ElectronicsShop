using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.CreateProduct.Images;

public class AddProductImagesCommandHandler:ResponseHandler,IRequestHandler<AddProductImageCommand,GenericResponse<Unit>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public AddProductImagesCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IFileService fileService)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }
    
    public async Task<GenericResponse<Unit>> Handle(AddProductImageCommand request, CancellationToken cancellationToken)
    {
        if(request.Images == null || !request.Images.Any())
        {
            return BadRequest<Unit>("No images provided");
        }
        
        var product = await _productRepository.GetProductByIdWithImages(request.ProductId);

        if (product is null)
        {
            return NotFound<Unit>("Product not found");
        }

        var savedImageUrls = new List<string>();
        
        foreach (var imageFile in request.Images)
        {
            try
            {
                // Save the image using the file service
                var imageUrl = await _fileService.SaveImageAsync(imageFile, "products");
                savedImageUrls.Add(imageUrl);

                // Call the domain entity's method to add the image URL
                var addImageResult = product.AddImage(imageUrl);
                if (addImageResult.IsError)
                {
                    // If any image fails a domain rule, we must roll back all file saves from this request.
                    CleanupSavedFiles(savedImageUrls);
                    return Conflict<Unit>(addImageResult.Errors.FirstOrDefault().Description);
                }
            }
            catch (Exception ex) // Catch potential exceptions from file saving
            {
                // On any file system error, roll back all file saves from this request.
                CleanupSavedFiles(savedImageUrls);
                return InternalServerError<Unit>($"An error occurred while saving an image: {ex.Message}");
            }
        }
        
        // Save changes to the database
        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success(Unit.Value,message:"Images added successfully");
    }


    #region Helpers Methods

    private void CleanupSavedFiles(List<string> fileUrls)
    {
        foreach (var url in fileUrls)
        {
            _fileService.DeleteImageAsync(url);
        }
    }

    #endregion
}