using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Users.Commands;

public sealed record LockUserCommand(Guid Id): IRequest<GenericResponse<Unit>>;