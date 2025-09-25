using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Users.Dtos;
using MediatR;

namespace ElectronicsShop.Application.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid Id): IRequest<GenericResponse<UserResponse>>;