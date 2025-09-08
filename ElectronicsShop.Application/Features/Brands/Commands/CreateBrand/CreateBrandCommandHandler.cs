using AutoMapper;
using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Domain.Products.Brands;
using MediatR;

namespace ElectronicsShop.Application.Features.Brands.Commands.CreateBrand;

public class CreateBrandCommandHandler:ResponseHandler,IRequestHandler<CreateBrandCommand,GenericResponse<int>>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
    {
        _brandRepository = brandRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<GenericResponse<int>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        // check if brand with the same name exists
        var isBrandExist = await _brandRepository.ExistsAsync(b => b.Name == request.Name);
        if (isBrandExist) return Conflict<int>("Brand already exists");

        // create brand 
        var brand = Brand.Create(request.Name, request.LogoUrl);

        if (brand.IsError)
        {
            return BadRequest<int>(brand.Errors.First().Description);
        }
        
        // add brand
        await _brandRepository.AddAsync(brand.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success(brand.Value.Id, "Brand Created Successfully");
    }
}