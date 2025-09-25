using FluentValidation;

namespace ElectronicsShop.Application.Features.Authentication.Commands.SigninUser;

public class SigninUserCommandValidator:AbstractValidator<SigninUserCommand>
{
    public SigninUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}