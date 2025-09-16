using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.DeleteProduct.DeleteImage;

public class DeleteProductImagesCommandHandler:ResponseHandler, IRequestHandler<DeleteProductImagesCommand,GenericResponse<Unit>>
{

    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    
    public DeleteProductImagesCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IFileService fileService)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }
    
    public async Task<GenericResponse<Unit>> Handle(DeleteProductImagesCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdWithImages(request.ProductId);

        if (product is null)
        {
            return NotFound<Unit>("Product not found");
        }

        var urlsToDelete = new List<string>();

        // Loop through the IDs of the images to be deleted
        foreach (var imageId in request.ImageIds)
        {
            var imageUrl = product.Images.FirstOrDefault(i => i.Id == imageId)?.Url;
            if (imageUrl is null)
            {
                // Optionally, you could collect these errors and return them all at once
                // For simplicity, we fail on the first invalid ID
                return NotFound<Unit>($"Image with ID {imageId} not found");
            }

            var removeResult = product.RemoveImage(imageId);
            if (removeResult.IsError)
            {
                return Conflict<Unit>(removeResult.Errors.FirstOrDefault().Description);
            }
            
            urlsToDelete.Add(imageUrl);
        }

        if (urlsToDelete.Count == 0)
        {
            return BadRequest<Unit>( "No valid image IDs were provided for deletion.");
        }

        // Persist the changes to the database FIRST
       _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // After a successful database save, delete the files from the file system
        foreach (var url in urlsToDelete)
        {
            await _fileService.DeleteImageAsync(url);
        }

        return Success(Unit.Value, "Images deleted successfully");
    }
}