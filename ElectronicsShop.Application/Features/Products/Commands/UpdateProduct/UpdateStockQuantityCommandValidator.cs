using FluentValidation;

namespace ElectronicsShop.Application.Features.Products.Commands.UpdateProduct;

public sealed class UpdateStockQuantityCommandValidator 
    : AbstractValidator<UpdateStockQuantityCommand>
{
    public UpdateStockQuantityCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Product Id must be greater than 0.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");
    }
}