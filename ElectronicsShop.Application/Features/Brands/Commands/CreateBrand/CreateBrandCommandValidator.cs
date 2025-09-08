using FluentValidation;

namespace ElectronicsShop.Application.Features.Brands.Commands.CreateBrand;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage("Brand name cannot be empty null");
        RuleFor(b => b.LogoUrl).NotEmpty().NotNull().WithMessage("Logo URL cannot be empty or null");
    }
}