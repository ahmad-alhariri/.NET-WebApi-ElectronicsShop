using ElectronicsShop.Application.Common.Models;
using ElectronicsShop.Application.Features.Carts.Dtos;
using MediatR;

namespace ElectronicsShop.Application.Features.Carts.Queries;

public sealed record GetCartQuery():IRequest<GenericResponse<CartResponse>>;