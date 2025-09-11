using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.UpdateProduct;

public sealed record UpdatePriceCommand(int ProductId, decimal Amount, string Currency)
    : IRequest<GenericResponse<Unit>>;