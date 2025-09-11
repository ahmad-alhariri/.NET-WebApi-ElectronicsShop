using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.UpdateProduct.UpdatePrimaryImage;

public sealed record SetPrimaryProductImageCommand(int ProductId, int ImageId) : IRequest<GenericResponse<Unit>>;
