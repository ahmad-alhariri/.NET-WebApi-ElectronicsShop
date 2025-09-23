using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Authentication.Dtos;
using MediatR;

namespace ElectronicsShop.Application.Features.Authentication.Commands.RefreshExpiredToken;

public record RefreshTokenCommand(
    string ExpiredAccessToken,
    string RefreshTokenString
): IRequest<GenericResponse<TokenResponse>>;