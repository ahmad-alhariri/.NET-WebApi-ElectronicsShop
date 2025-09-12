using FluentValidation;

namespace ElectronicsShop.Application.Features.Products.Commands.CreateProduct;

public sealed class AddOrUpdateSpecificationsCommandValidator:AbstractValidator<AddOrUpdateSpecificationsCommand>
{
    public AddOrUpdateSpecificationsCommandValidator()
    {
        RuleFor(p => p.ProductId)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Product ID required and must be a positive integer.");
        
        
        
        
        RuleForEach(p => p.Specifications).ChildRules(spec =>
        {
            spec.RuleFor(s => s.Key)
                .NotEmpty().WithMessage("Specification key is required.")
                .MaximumLength(100).WithMessage("Specification key must not exceed 100 characters.");
            
            spec.RuleFor(s => s.Value)
                .NotEmpty().WithMessage("Specification value is required.")
                .MaximumLength(500).WithMessage("Specification value must not exceed 500 characters.");
        });
    }
}