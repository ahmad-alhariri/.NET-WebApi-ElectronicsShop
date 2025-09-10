using FluentValidation;

namespace ElectronicsShop.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator:AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Category Id is required")
            .GreaterThan(0).WithMessage("Category Id must be greater than 0");

        // Fields are optional now
        RuleFor(c => c.Name)
            .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");

        RuleFor(c => c.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(c => c.ImageFile)
            .Must(file => file == null || file.Length > 0)
            .WithMessage("If provided, image file must not be empty.");

        // Custom rule: at least one property must be provided
        RuleFor(x => x)
            .Must(x =>
                !string.IsNullOrWhiteSpace(x.Name) ||
                !string.IsNullOrWhiteSpace(x.Description) ||
                x.ImageFile != null)
            .WithMessage("At least one field (Name, Description, or ImageFile) must be provided for update.");

    }
}