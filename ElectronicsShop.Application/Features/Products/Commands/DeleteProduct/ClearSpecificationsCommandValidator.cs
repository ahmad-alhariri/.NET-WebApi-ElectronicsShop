using FluentValidation;

namespace ElectronicsShop.Application.Features.Products.Commands.DeleteProduct;

public class ClearSpecificationsCommandValidator: AbstractValidator<ClearSpecificationsCommand>
{
    public ClearSpecificationsCommandValidator()
    {
        RuleFor(p => p.ProductId)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Product ID required and must be a positive integer.");
    }
}