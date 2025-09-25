using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Authentication.Queries;

public sealed record ConfirmEmailQuery( string UserEmail, string Token): IRequest<GenericResponse<Unit>>;