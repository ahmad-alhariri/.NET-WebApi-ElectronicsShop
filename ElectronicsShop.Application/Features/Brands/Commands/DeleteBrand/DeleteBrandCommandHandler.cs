using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Interfaces.Repositories;
using MediatR;

namespace ElectronicsShop.Application.Features.Brands.Commands.DeleteBrand;

public class DeleteBrandCommandHandler: ResponseHandler, IRequestHandler<DeleteBrandCommand,GenericResponse<bool>>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBrandCommandHandler(IBrandRepository repository, IUnitOfWork unitOfWork)
    {
        _brandRepository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<GenericResponse<bool>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        // check if brand exist
        var existingBrand = await _brandRepository.GetByIdAsync(request.Id);
        if (existingBrand == null) return NotFound<bool>("Brand does not exist");

        // delete brand
        _brandRepository.Remove(existingBrand);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Deleted<bool>("Brand deleted successfully");
    }
}