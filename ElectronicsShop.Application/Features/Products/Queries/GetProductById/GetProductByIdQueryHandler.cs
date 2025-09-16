using AutoMapper;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler:ResponseHandler,IRequestHandler<GetProductByIdQuery,GenericResponse<ProductResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    
    public async Task<GenericResponse<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        // get product with includes
        var product = await _productRepository.GetByIdWithIncludesAsync(request.Id);
        
        // check if product exists
        if (product == null)
        {
            return NotFound<ProductResponse>($"Product with ID ({request.Id}) not found");
        }
        
        // map to response dto
        var response = _mapper.Map<ProductResponse>(product);
        
        // return success response
        return Success(response, $"Product with ID ({request.Id}) retrieved successfully.");
    }
}