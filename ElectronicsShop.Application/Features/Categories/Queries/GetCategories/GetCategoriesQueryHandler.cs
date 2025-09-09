using AutoMapper;
using ElectronicsShop.Application.Common.Extensions;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Brands.Dtos;
using ElectronicsShop.Application.Features.Categories.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler:ResponseHandler,IRequestHandler<GetCategoriesQuery,GenericResponse<List<CategoryResponse>>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    public async Task<GenericResponse<List<CategoryResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _categoryRepository.GetAllAsNoTracking().AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var normalized = request.SearchTerm.Trim().ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(normalized));
        }

        var isDescending = request.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase);
        
        query = request.SortColumn.ToLower() switch
        {
            "createdat" => isDescending ? query.OrderByDescending(c => c.CreatedDate) : query.OrderBy(c => c.CreatedDate),
            "name" => isDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
            _ => query.OrderByDescending(c => c.CreatedDate) // Default sorting
        };

        var pagedCategories = await query.ToPagedListAsync(request.Page, request.PageSize, cancellationToken);

        var categoryItems = _mapper.Map<List<CategoryResponse>>(pagedCategories.Items);
        
        return Paginated(categoryItems, pagedCategories.TotalCount, pagedCategories.PageNumber, pagedCategories.PageSize, "Brands retrieved successfully");
    }
}