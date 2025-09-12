using FluentValidation;

namespace ElectronicsShop.Application.Features.Products.Commands.UpdateProduct;

public class SetProductFeaturedStatusCommandValidator:AbstractValidator<SetProductFeaturedStatusCommand>
{
    public SetProductFeaturedStatusCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();
        
        RuleFor(x => x.IsFeatured)
            .NotNull();
    }
}