using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.DeleteProduct.DeleteImage;

public sealed record DeleteProductImageCommand(int ProductId, int ImageId) : IRequest<GenericResponse<Unit>>;
