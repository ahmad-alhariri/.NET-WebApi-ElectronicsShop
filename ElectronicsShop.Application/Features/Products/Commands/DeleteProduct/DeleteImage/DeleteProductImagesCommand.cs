using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.DeleteProduct.DeleteImage;

public sealed record DeleteProductImagesCommand(
    int ProductId,
    List<int> ImageIds) : IRequest<GenericResponse<Unit>>;