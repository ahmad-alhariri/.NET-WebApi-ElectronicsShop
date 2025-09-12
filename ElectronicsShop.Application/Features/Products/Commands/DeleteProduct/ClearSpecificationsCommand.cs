using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.DeleteProduct;

public sealed record ClearSpecificationsCommand(
    int ProductId
) : IRequest<GenericResponse<Unit>>;