using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(int Id): IRequest<GenericResponse<bool>>;