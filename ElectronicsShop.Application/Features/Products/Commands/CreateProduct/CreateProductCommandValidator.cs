using FluentValidation;

namespace ElectronicsShop.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator:AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(200).WithMessage("Product name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Product description is required.")
            .MaximumLength(1000).WithMessage("Product description must not exceed 2000 characters.");

        RuleFor(x => x.PriceAmount)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.PriceCurrency)
            .NotEmpty().WithMessage("Currency is required.")
            .Length(3).WithMessage("Currency must be a valid ISO 4217 3-letter code (e.g., USD, EUR).");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity must be greater than or equal to zero.");

        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(50).WithMessage("SKU must not exceed 50 characters.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Valid category is required.");

        RuleFor(x => x.BrandId)
            .GreaterThan(0).WithMessage("Valid brand is required.");

        // Specifications (optional)
        When(x => x.Specifications != null, () =>
        {
            RuleForEach(x => x.Specifications)
                .NotEmpty()
                .WithMessage("Specification key cannot be empty.");
        });

        // Images (required, at least one)
        RuleFor(x => x.Images)
            .NotEmpty().WithMessage("At least one product image is required.");

        RuleForEach(x => x.Images)
            .Must(file => file.Length > 0)
            .WithMessage("Uploaded image cannot be empty.");
    }
}