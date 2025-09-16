namespace ElectronicsShop.Application.Features.Products.Dtos;

public record ProductImportResult(
    string Name,
    bool Success,
    string? ErrorMessage
);