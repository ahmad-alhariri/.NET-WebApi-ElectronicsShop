using FluentValidation;

namespace ElectronicsShop.Application.Features.Authentication.Commands.ChangeUserPassword;

public class ForgotPasswordCommandValidator: AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");
    }
}