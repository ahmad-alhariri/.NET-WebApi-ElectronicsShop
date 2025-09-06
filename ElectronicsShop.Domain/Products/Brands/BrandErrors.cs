using ElectronicsShop.Domain.Common.Results;

namespace ElectronicsShop.Domain.Products.Brands;

public static class BrandErrors
{
    public static Error NameRequired => Error.Validation("Brand_Name_Required", "Brand name is required.");
}