using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Authentication.Commands.ChangeUserPassword;

public sealed record ForgotPasswordCommand(string Email): IRequest<GenericResponse<Unit>>;