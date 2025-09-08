using FluentValidation;

namespace ElectronicsShop.Application.Features.Brands.Commands.DeleteBrand;

public class DeleteBrandCommandValidator : AbstractValidator<DeleteBrandCommand>
{
    public DeleteBrandCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Brand Id is required.");
    }

}