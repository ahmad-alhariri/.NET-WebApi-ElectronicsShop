using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Brands.Commands.UpdateBrand;

public class UpdateBrandCommandHandler:ResponseHandler, IRequestHandler<UpdateBrandCommand, GenericResponse<int>>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
    {
        _brandRepository = brandRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<GenericResponse<int>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {

        // check if the brand exist
        var existingBrand = await _brandRepository.GetByIdAsync(request.Id);
        if (existingBrand == null) return NotFound<int>("Brand does not exist");

        // check if name exist
        var isBrandNameExist =
            await _brandRepository.ExistsAsync(b => b.Name == request.Name && b.Id != request.Id);
        if (isBrandNameExist) return Conflict<int>("Brand name already exists");

        
        // update brand
        var result = existingBrand.UpdateDetails(request.Name, request.LogoUrl);
        if (result.IsError)
        {
            return BadRequest<int>(result.Errors.FirstOrDefault().Description);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Success(existingBrand.Id, "Brand updated successfully");
        
    }
}