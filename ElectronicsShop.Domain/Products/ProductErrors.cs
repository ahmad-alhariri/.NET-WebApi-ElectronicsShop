using ElectronicsShop.Domain.Common.Results;

namespace ElectronicsShop.Domain.Products;

public static class ProductErrors
{
    public static Error NameRequired => Error.Validation("Product_Name_Required", "Product name is required");
    
    public static Error DescriptionRequired => Error.Validation("Description_Name_Required", "Description name is required");
    public static Error SkuRequired => Error.Validation("Sku_Name_Required", "SKU name is required");
    
    public static Error CategoryRequired => Error.Validation("Category_Name_Required", "Category name is required");
    public static Error BrandRequired => Error.Validation("Brand_Name_Required", "Brand name is required");
    public static Error PriceGreaterThanZero =>
        Error.Validation("Price_GreaterThan_Zero", "Price must be greater than zero");
    
    public static Error StockQuantityGreaterThanZero =>
        Error.Validation("StockQuantity_GreaterThan_Zero", "Stock quantity must be greater than zero");    
    public static Error InsufficientStock =>
        Error.Validation("Insufficient_Stock","Insufficient stock for adjustment");
    public static Error ImageNotFount =>
        Error.Validation("Image_Not_Found","Image not found on this product");   
    public static Error ImageUrlRequired =>
        Error.Validation("Image_Url_Required","Image URL is required"); 
    public static Error DuplicateImage =>
        Error.Validation("Duplicate_Image","Duplicate image URL for this product");
    
    public static Error SpecificationKeyRequired =>
        Error.Validation("Specification_Key_Required","Specification key is required");
}