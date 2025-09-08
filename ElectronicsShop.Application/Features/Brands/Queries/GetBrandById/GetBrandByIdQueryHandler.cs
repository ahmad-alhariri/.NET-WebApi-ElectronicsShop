using AutoMapper;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Brands.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Brands.Queries.GetBrandById;

public class GetBrandByIdQueryHandler:ResponseHandler, IRequestHandler<GetBrandByIdQuery, GenericResponse<BrandResponse>>
{
    private readonly IMapper _mapper;
    private readonly IBrandRepository _brandRepository;

    public GetBrandByIdQueryHandler(IMapper mapper, IBrandRepository brandRepository)
    {
        _mapper = mapper;
        _brandRepository = brandRepository;
    }
    public async Task<GenericResponse<BrandResponse>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
    {
        // check if brand exists
        var brand = await _brandRepository.GetByIdAsync(request.Id);
        if (brand == null) return NotFound<BrandResponse>("Brand does not exist");

        // map to response
        var response = _mapper.Map<BrandResponse>(brand);
        return Success(response, "Brand Retrieved Successfully");
    }
}