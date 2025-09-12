using ElectronicsShop.Application.Common.Models;
using MediatR;

namespace ElectronicsShop.Application.Features.Products.Commands.DeleteProduct;

public sealed record RemoveSpecificationCommand(int ProductId, string SpecificationKey):IRequest<GenericResponse<Unit>>;