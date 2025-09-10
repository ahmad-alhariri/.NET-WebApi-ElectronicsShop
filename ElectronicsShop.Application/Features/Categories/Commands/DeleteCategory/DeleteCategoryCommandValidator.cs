using FluentValidation;

namespace ElectronicsShop.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandValidator:AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Category Id is required")
            .GreaterThan(0).WithMessage("Category Id must be greater than 0");
    }
}