using FluentValidation;

namespace ElectronicsShop.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandValidator:AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithMessage("Product Id must be greater than 0");
    }
}