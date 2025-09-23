using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Authentication.Commands.LogoutUser;

public record LogoutUserCommand(string RefreshToken) : IRequest<GenericResponse<Unit>>;