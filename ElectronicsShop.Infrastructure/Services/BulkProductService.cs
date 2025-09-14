using ClosedXML.Excel;
using ElectronicsShop.Application.Features.Products.Dtos;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Application.Interfaces.Services;
using ElectronicsShop.Domain.Common.ValueObjects;
using ElectronicsShop.Domain.Products;

namespace ElectronicsShop.Infrastructure.Services;

public class BulkProductService : IBulkProductService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BulkProductService(
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public byte[] ExportProducts(IEnumerable<ProductExportDto> products)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Products");


        // Headers
        worksheet.Cell(1, 1).Value = "Name";
        worksheet.Cell(1, 2).Value = "Description";
        worksheet.Cell(1, 3).Value = "Price";
        worksheet.Cell(1, 4).Value = "Currency";
        worksheet.Cell(1, 5).Value = "Stock";
        worksheet.Cell(1, 6).Value = "SKU";
        worksheet.Cell(1, 7).Value = "CategoryId";
        worksheet.Cell(1, 8).Value = "Category";
        worksheet.Cell(1, 9).Value = "BrandId";
        worksheet.Cell(1, 10).Value = "Brand";

        worksheet.Cell(1, 11).Value = "IsActive";
        worksheet.Cell(1, 12).Value = "IsFeatured";

        worksheet.Range("A1:L1").Style.Font.Bold = true;
        worksheet.Range("A1:L1").Style.Fill.BackgroundColor = XLColor.LightGray;
        worksheet.Range("A1:L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Range("A1:L1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


        var row = 2;
        foreach (var p in products)
        {
            worksheet.Cell(row, 1).Value = p.Name;
            worksheet.Cell(row, 2).Value = p.Description;
            worksheet.Cell(row, 3).Value = p.Price;
            worksheet.Cell(row, 4).Value = p.Currency;
            worksheet.Cell(row, 5).Value = p.StockQuantity;
            worksheet.Cell(row, 6).Value = p.Sku;
            worksheet.Cell(row, 7).Value = p.CategoryId;
            worksheet.Cell(row, 8).Value = p.Category;
            worksheet.Cell(row, 9).Value = p.BrandId;
            worksheet.Cell(row, 10).Value = p.Brand;
            worksheet.Cell(row, 11).Value = p.IsActive;
            worksheet.Cell(row, 11).Style.Fill.BackgroundColor = p.IsActive ? XLColor.AppleGreen : XLColor.OrangeRed;
            worksheet.Cell(row, 12).Value = p.IsFeatured;
            worksheet.Cell(row, 12).Style.Fill.BackgroundColor = p.IsFeatured ? XLColor.AppleGreen : XLColor.OrangeRed;

            row++;
        }

        worksheet.Column(2).Width = 50;
        worksheet.Column(2).Style.Alignment.WrapText = true;
        worksheet.Columns().Width = 20;
        worksheet.Columns().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Columns().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        worksheet.Columns().AdjustToContents();


        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<IEnumerable<ProductImportResult>> ImportProductsAsync(Stream fileStream, string contentType,
        CancellationToken cancellationToken = default)
    {
        var results = new List<ProductImportResult>();

        using var workbook = new XLWorkbook(fileStream);
        var worksheet = workbook.Worksheet(1);

        foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header
        {
            var name = row.Cell(1).GetString().Trim();
            var description = row.Cell(2).GetString().Trim();
            var priceValue = row.Cell(3).GetValue<decimal>();
            var stock = row.Cell(5).GetValue<int>();
            var sku = row.Cell(6).GetString().Trim();
            var categoryName = row.Cell(8).GetString().Trim();
            var brandName = row.Cell(10).GetString().Trim();
            var isFeatured = row.Cell(12).GetBoolean();

            try
            {
                // Validate category
                var category = await _categoryRepository.GetByNameAsync(categoryName, cancellationToken);
                if (category is null)
                {
                    results.Add(new ProductImportResult(name, false, $"Category '{categoryName}' not found"));
                    continue;
                }

                // Validate brand
                var brand = await _brandRepository.GetByNameAsync(brandName, cancellationToken);
                if (brand is null)
                {
                    results.Add(new ProductImportResult(name, false, $"Brand '{brandName}' not found"));
                    continue;
                }

                // Check if product exists (by SKU)
                var existing = await _productRepository.GetBySkuAsync(sku, cancellationToken);

                if (existing is not null)
                {
                    // Update existing product
                    var updateResult = existing.UpdateDetails(
                        name,
                        description,
                        new Money(priceValue, "USD"),
                        sku,
                        category.Id,
                        brand.Id
                    );

                    if (updateResult.IsError)
                    {
                        results.Add(new ProductImportResult(name, false,
                            updateResult.Errors.FirstOrDefault().Description));
                        continue;
                    }


                    existing.UpdateStock(stock);
                    if (isFeatured)
                        existing.MarkAsFeatured();

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    results.Add(new ProductImportResult(name, true, "Updated"));
                }
                else
                {
                    // Create new product
                    var productResult = Product.Create(
                        name,
                        description,
                        new Money(priceValue, "USD"),
                        stock,
                        sku,
                        category.Id,
                        brand.Id
                    );

                    if (productResult.IsError)
                    {
                        results.Add(new ProductImportResult(name, false,
                            productResult.Errors.FirstOrDefault().Description));
                        continue;
                    }

                    var product = productResult.Value;

                    if (isFeatured)
                    {
                        product.MarkAsFeatured();
                    }

                    await _productRepository.AddAsync(product, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    results.Add(new ProductImportResult(name, true, "Created"));
                }
            }
            catch (Exception ex)
            {
                results.Add(new ProductImportResult(name, false, ex.Message));
            }
        }

        return results;
    }
}