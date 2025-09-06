using System.Text.Json;
using ElectronicsShop.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicsShop.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        // Configure the Money Value Object as an Owned Entity Type
        builder.OwnsOne(p => p.Price, priceBuilder =>
        {
            priceBuilder.Property(m => m.Amount)
                .HasColumnName("PriceAmount")
                .HasColumnType("decimal(18,2)");

            priceBuilder.Property(m => m.Currency)
                .HasColumnName("PriceCurrency")
                .HasMaxLength(3);
        });
        
        // Configure the Specifications property
        builder.Property(p => p.Specifications)
            .HasColumnName("SpecificationsJson") // The database column name
            .HasConversion(
                // Convert the Dictionary to a JSON string for storing in the DB
                specs => JsonSerializer.Serialize(specs, (JsonSerializerOptions?)null),
                
                // Convert the JSON string back to a Dictionary when reading from the DB
                json => JsonSerializer.Deserialize<Dictionary<string, string>>(json, (JsonSerializerOptions?)null) ?? new Dictionary<string, string>(),

                // Value comparer to help EF Core track changes correctly
                new ValueComparer<IReadOnlyDictionary<string, string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value.GetHashCode()))
                )
            );
        
        // Configure the one-to-many relationship with Category
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the one-to-many relationship with Brand
        builder.HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationship with ProductImage
        builder.HasMany(p => p.Images)
            .WithOne() // Assuming ProductImage doesn't have a navigation property back to Product
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade); // Delete images if the product is deleted
    }
}
