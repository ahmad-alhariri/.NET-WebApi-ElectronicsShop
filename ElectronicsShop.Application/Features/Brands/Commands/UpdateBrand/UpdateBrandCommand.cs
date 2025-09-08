using ElectronicsShop.Application.Common.Models;
using MediatR;


namespace ElectronicsShop.Application.Features.Brands.Commands.UpdateBrand;

public sealed record UpdateBrandCommand(int Id, string Name, string LogoUrl) : IRequest<GenericResponse<int>>;