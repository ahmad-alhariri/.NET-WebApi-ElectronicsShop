using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Authentication.Commands.ChangeUserPassword;

public record ChangePasswordCommand(
    string OldPassword,
    string NewPassword
) : IRequest<GenericResponse<Unit>>;