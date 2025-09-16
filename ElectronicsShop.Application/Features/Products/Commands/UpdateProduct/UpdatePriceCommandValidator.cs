using FluentValidation;

namespace ElectronicsShop.Application.Features.Products.Commands.UpdateProduct;

public class UpdatePriceCommandValidator:AbstractValidator<UpdatePriceCommand>
{
    public UpdatePriceCommandValidator()
    {
        RuleFor(p => p.ProductId)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Product ID required and must be a positive integer.");
        
        RuleFor(p => p.Amount)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Price amount must be greater than zero.");
        
    }
}