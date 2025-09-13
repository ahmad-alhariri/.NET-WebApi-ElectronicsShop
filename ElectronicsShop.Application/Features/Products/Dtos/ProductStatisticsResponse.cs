namespace ElectronicsShop.Application.Features.Products.Dtos;

public sealed record ProductStatisticsResponse(
    int TotalProducts,
    int ActiveProducts,
    int InactiveProducts,
    int LowStockProducts,
    int OutOfStockProducts
);