using FluentValidation;

namespace ElectronicsShop.Application.Features.Authentication.Commands.RefreshExpiredToken;

public class RefreshTokenCommandValidator:AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.ExpiredAccessToken)
            .NotEmpty().WithMessage("Expired access token is required.")
            .NotNull().WithMessage("Expired access token cannot be null.");

        RuleFor(x => x.RefreshTokenString)
            .NotEmpty().WithMessage("Refresh token is required.")
            .NotNull().WithMessage("Refresh token cannot be null.");
    }
}