using ElectronicsShop.Application.Common.Models;
using MediatR;


namespace ElectronicsShop.Application.Features.Brands.Commands.DeleteBrand;

public sealed record DeleteBrandCommand(int Id) : IRequest<GenericResponse<bool>>;
