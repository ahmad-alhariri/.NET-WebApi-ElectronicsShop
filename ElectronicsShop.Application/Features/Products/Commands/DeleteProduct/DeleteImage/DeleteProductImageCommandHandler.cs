using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Common.Results;
using ElectronicsShop.Domain.Products;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.DeleteProduct.DeleteImage;

public class DeleteProductImageCommandHandler: ResponseHandler, IRequestHandler<DeleteProductImageCommand, GenericResponse<Unit>>
{
    private readonly IProductRepository _productRepository;
    private readonly IFileService _fileService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductImageCommandHandler(IProductRepository productRepository, IFileService fileService, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _fileService = fileService;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<GenericResponse<Unit>> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdWithImages(request.ProductId);

        if (product is null)
        {
            return NotFound<Unit>("Product not found");
        }

        // Find the specific image to get its URL before it's removed from the domain entity
        var imageUrl = product.Images.FirstOrDefault(i => i.Id == request.ImageId)?.Url;
        if (imageUrl is null)
        {
            return NotFound<Unit>($"Image with ID {request.ImageId} not found");
        }

        // Call the domain entity's method to remove the image from the collection
        var removeResult = product.RemoveImage(request.ImageId);
        if (removeResult.IsError)
        {
            return Conflict<Unit>(removeResult.Errors.FirstOrDefault().Description);
        }

        // Persist the changes to the database FIRST for transactional integrity
        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // After a successful database save, delete the physical file from the server
        await _fileService.DeleteImageAsync(imageUrl);

        return Success(Unit.Value, "Image deleted successfully");
    }
}