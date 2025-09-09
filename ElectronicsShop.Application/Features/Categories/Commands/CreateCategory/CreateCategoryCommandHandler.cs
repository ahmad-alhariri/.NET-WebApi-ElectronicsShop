using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Products.Categories;
using MediatR;

namespace ElectronicsShop.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler:ResponseHandler, IRequestHandler<CreateCategoryCommand, GenericResponse<int>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork,IFileService fileService)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }
    
    public async Task<GenericResponse<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        // check if category exists
        var existingCategory = await _categoryRepository.ExistsAsync(c => c.Name ==request.CategoryName);
        if (existingCategory)
        {
            return Conflict<int>("Category with the same name already exists");
        }

        // create new category
        string imageUrl = string.Empty;
        if (request.ImageFile != null)
        {
            imageUrl = await _fileService.SaveImageAsync(request.ImageFile, "Categories");
        }
        
        var newCategory = Category.Create(request.CategoryName, request.Description, imageUrl);

        if (newCategory.IsError)
        {
            return BadRequest<int>(newCategory.Errors.FirstOrDefault().Description);
        }

        await _categoryRepository.AddAsync(newCategory.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success(newCategory.Value.Id, "Category created successfully");
    }
}