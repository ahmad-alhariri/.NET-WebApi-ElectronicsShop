using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Carts.Commands;

public sealed record RemoveCartItemCommand(int ProductId):IRequest<GenericResponse<Unit>>;