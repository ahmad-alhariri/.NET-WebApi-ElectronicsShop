using FluentValidation;

namespace ElectronicsShop.Application.Features.Authentication.Commands.ChangeUserPassword;

public class ChangePasswordCommandValidator:AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Old password is required.")
            .MinimumLength(6).WithMessage("Old password must be at least 6 characters long.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(6).WithMessage("New password must be at least 6 characters long.")
            .NotEqual(x => x.OldPassword).WithMessage("New password must be different from the old password.");
    }
}