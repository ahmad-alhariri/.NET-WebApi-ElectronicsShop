using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Brands.Dtos;
using MediatR;

namespace ElectronicsShop.Application.Features.Brands.Queries.GetBrandById;

public sealed record GetBrandByIdQuery(int Id):IRequest<GenericResponse<BrandResponse>>;