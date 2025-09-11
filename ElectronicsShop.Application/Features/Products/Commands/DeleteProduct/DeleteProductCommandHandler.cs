using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler: ResponseHandler, IRequestHandler<DeleteProductCommand, GenericResponse<bool>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork,IFileService fileService)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }
    
    public async Task<GenericResponse<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdWithImages(request.Id);
        if (product == null)
        {
            return NotFound<bool>("Product not found");
        }
        
        var imagePaths = product.Images.Select(img => img.Url);
        foreach (var path in imagePaths)
        {
            await _fileService.DeleteImageAsync(path);
        }

        _productRepository.Remove(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success(true, "Product deleted successfully");
    }
}