using ElectronicsShop.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ElectronicsShop.Application.Features.Products.Commands.CreateProduct.Images;

public sealed record AddProductImageCommand(
    int ProductId,
    List<IFormFile> Images) : IRequest<GenericResponse<Unit>>;