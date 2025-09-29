using FluentValidation;

namespace ElectronicsShop.Application.Features.Carts.Commands;

public class UpdateCartItemCommandValidator:AbstractValidator<UpdateCartItemCommand>
{
    public UpdateCartItemCommandValidator()
    {
        RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("ProductId must be greater than 0");
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal 0");
    }
}