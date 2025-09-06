using ElectronicsShop.Domain.Common.Results;

namespace ElectronicsShop.Domain.Products.Categories;

public static class CategoryErrors
{
    public static Error NameRequired => Error.Validation("Category_Name_Required", "Category name is required.");
}