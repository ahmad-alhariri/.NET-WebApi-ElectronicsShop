using AutoMapper;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Categories.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler:ResponseHandler, IRequestHandler<GetCategoryByIdQuery, GenericResponse<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    public async Task<GenericResponse<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        // check if brand exists
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category == null) return NotFound<CategoryResponse>("Brand does not exist");

        // map to response
        var response = _mapper.Map<CategoryResponse>(category);
        return Success(response, "Brand Retrieved Successfully");
    }
}