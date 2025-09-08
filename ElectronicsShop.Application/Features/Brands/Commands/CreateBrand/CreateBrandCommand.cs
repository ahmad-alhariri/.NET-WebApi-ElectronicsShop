using ElectronicsShop.Application.Common.Models;
using MediatR;


namespace ElectronicsShop.Application.Features.Brands.Commands.CreateBrand;

public sealed record CreateBrandCommand(string Name, string LogoUrl) : IRequest<GenericResponse<int>>;
