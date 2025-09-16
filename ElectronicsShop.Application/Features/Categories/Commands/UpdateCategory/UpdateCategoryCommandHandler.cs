using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using MediatR;

namespace ElectronicsShop.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler:ResponseHandler, IRequestHandler<UpdateCategoryCommand, GenericResponse<int>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IFileService fileService)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }
    
    public async Task<GenericResponse<int>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        // 1. Check if category exists
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category == null)
        {
            return NotFound<int>($"Category with Id {request.Id} not found");
        }

        // 2. Check if new name already exists (excluding current category)
        var exists = await _categoryRepository.ExistsAsync(
            c => c.Name == request.Name && c.Id != request.Id,cancellationToken);

        if (exists)
        {
            return Conflict<int>("Another category with the same name already exists");
        }

        // 3. Handle image
        var imageUrl = category.ImageUrl;
        if (request.ImageFile != null)
        {
            // Optionally delete old image
            if (!string.IsNullOrEmpty(imageUrl))
            {
                await _fileService.DeleteImageAsync(imageUrl);
            }

            imageUrl = await _fileService.SaveImageAsync(request.ImageFile, "Categories");
        }

        // 4. Update entity using domain method
        var result = category.UpdateDetails(request.Name, request.Description, imageUrl);

        if (result.IsError)
        {
            return BadRequest<int>(result.Errors.FirstOrDefault().Description ?? "Invalid data");
        }

        // 5. Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success(category.Id, "Category updated successfully");
    }
}