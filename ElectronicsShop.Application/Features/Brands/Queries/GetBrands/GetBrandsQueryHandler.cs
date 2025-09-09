using AutoMapper;
using ElectronicsShop.Application.Common.Extensions;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Brands.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Brands.Queries.GetBrands;

public class GetBrandsQueryHandler:ResponseHandler,IRequestHandler<GetBrandsQuery,GenericResponse<List<BrandResponse>>>
{
    private readonly IMapper _mapper;
    private readonly IBrandRepository _brandRepository;

    public GetBrandsQueryHandler(IMapper mapper, IBrandRepository brandRepository)
    {
        _mapper = mapper;
        _brandRepository = brandRepository;
    }
    public async Task<GenericResponse<List<BrandResponse>>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
    {
        var query = _brandRepository.GetAllAsNoTracking().AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var normalized = request.SearchTerm.Trim().ToLower();
            query = query.Where(b => b.Name.ToLower().Contains(normalized));
        }

        var isDescending = request.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase);
        
        query = request.SortColumn.ToLower() switch
        {
            "createdat" => isDescending ? query.OrderByDescending(wo => wo.CreatedDate) : query.OrderBy(wo => wo.CreatedDate),
            "name" => isDescending ? query.OrderByDescending(wo => wo.Name) : query.OrderBy(wo => wo.Name),
            _ => query.OrderByDescending(wo => wo.CreatedDate) // Default sorting
        };

        var pagedBrands = await query.ToPagedListAsync(request.Page, request.PageSize, cancellationToken);

        var brandResponses = _mapper.Map<List<BrandResponse>>(pagedBrands.Items);
        
        return Paginated(brandResponses, pagedBrands.TotalCount, pagedBrands.PageNumber, pagedBrands.PageSize, "Brands retrieved successfully");


    }
}