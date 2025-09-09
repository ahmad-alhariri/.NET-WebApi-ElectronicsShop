using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using MediatR;

namespace ElectronicsShop.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler:ResponseHandler, IRequestHandler<DeleteCategoryCommand,GenericResponse<bool>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IFileService fileService)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }
    
    public async Task<GenericResponse<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        // check if category exists
        var existingCategory = await _categoryRepository.GetByIdAsync(request.Id);
        if (existingCategory == null)
        {
            return NotFound<bool>("Category not found");
        }
        
        // delete category
        // Delete image if it exists
        if (!string.IsNullOrEmpty(existingCategory.ImageUrl)) await _fileService.DeleteImageAsync(existingCategory.ImageUrl);
        
        _categoryRepository.Remove(existingCategory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Deleted<bool>( "Category deleted successfully");
    }
}