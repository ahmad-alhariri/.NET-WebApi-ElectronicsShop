using FluentValidation;

namespace ElectronicsShop.Application.Features.Brands.Commands.UpdateBrand;

public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    public UpdateBrandCommandValidator()
    {
        RuleFor(b => b.Id).NotEmpty().WithMessage("Id cannot be empty");
        RuleFor(b => b.Name).NotEmpty().WithMessage("Brand name cannot be empty");
        RuleFor(b => b.LogoUrl).NotEmpty().WithMessage("Logo URL cannot be empty");
    }
}