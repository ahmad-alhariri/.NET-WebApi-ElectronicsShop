using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Authentication.Dtos;
using MediatR;

namespace ElectronicsShop.Application.Features.Authentication.Commands;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<GenericResponse<Guid>>;