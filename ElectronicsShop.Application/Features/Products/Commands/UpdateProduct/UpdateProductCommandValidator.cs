using FluentValidation;

namespace ElectronicsShop.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator: AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Product ID required and must be a positive integer.");
        
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(200).WithMessage("Product name must not exceed 100 characters.");

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Product description is required.")
            .MaximumLength(1000).WithMessage("Product description must not exceed 1000 characters.");

        RuleFor(p => p.PriceAmount)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Price amount must be greater than zero.");
        

        RuleFor(p => p.Sku)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(50).WithMessage("SKU must not exceed 50 characters.");

        RuleFor(p => p.CategoryId)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Category ID must be a positive integer.");

        RuleFor(p => p.BrandId)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Brand ID must be a positive integer.");
        
    }
}