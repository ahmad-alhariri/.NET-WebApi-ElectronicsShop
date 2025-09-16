using MediatR;

namespace ElectronicsShop.Application.Features.Products.Queries.ExportProducts;

public sealed record ExportProductsQuery : IRequest<byte[]>;