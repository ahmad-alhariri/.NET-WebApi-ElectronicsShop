using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Carts.Commands;

public sealed record ClearCartCommand(): IRequest<GenericResponse<Unit>>;