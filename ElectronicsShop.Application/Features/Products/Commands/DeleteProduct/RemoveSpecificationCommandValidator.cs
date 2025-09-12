using FluentValidation;

namespace ElectronicsShop.Application.Features.Products.Commands.DeleteProduct;

public class RemoveSpecificationCommandValidator: AbstractValidator<RemoveSpecificationCommand>
{
    public RemoveSpecificationCommandValidator()
    {
        RuleFor(p => p.ProductId)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Product ID required and must be a positive integer.");

        RuleFor(p => p.SpecificationKey)
            .NotEmpty().WithMessage("Specification key is required.")
            .MaximumLength(100).WithMessage("Specification key must not exceed 100 characters.");
    }
}