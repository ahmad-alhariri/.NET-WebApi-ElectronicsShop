using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Domain.Common.Results;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.UpdateProduct;

public sealed record SetProductFeaturedStatusCommand(
    int ProductId,
    bool IsFeatured) : IRequest<GenericResponse<Unit>>;