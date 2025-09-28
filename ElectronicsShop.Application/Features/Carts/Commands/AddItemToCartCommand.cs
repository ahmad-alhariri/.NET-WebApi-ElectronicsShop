using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Carts.Commands;

public sealed record AddItemToCartCommand(int ProductId, int Quantity):IRequest<GenericResponse<Unit>>;