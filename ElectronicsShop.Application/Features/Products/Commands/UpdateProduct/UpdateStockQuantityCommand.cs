using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.UpdateProduct;

public sealed record UpdateStockQuantityCommand(
    int ProductId,
    int Quantity
) : IRequest<GenericResponse<Unit>>;