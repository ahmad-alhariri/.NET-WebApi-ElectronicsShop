using ElectronicsShop.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ElectronicsShop.Application.Features.Products.Commands.ImportProducts;

public record ImportProductsCommand(IFormFile File) : IRequest<IEnumerable<ProductImportResult>>;