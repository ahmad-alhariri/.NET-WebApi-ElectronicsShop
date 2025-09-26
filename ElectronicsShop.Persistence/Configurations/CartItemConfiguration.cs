using ElectronicsShop.Domain.Carts;
using ElectronicsShop.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicsShop.Persistence.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        // Primary Key
        builder.HasKey(ci => ci.Id);
        
        // Properties
        builder.Property(ci => ci.CartId)
            .IsRequired();
            
        builder.Property(ci => ci.ProductId)
            .IsRequired();
            
        builder.Property(ci => ci.Quantity)
            .IsRequired()
            .HasDefaultValue(1);

        // Money Value Object configuration
        builder.OwnsOne(ci => ci.Price, priceBuilder =>
        {
            priceBuilder.Property(m => m.Amount)
                .HasColumnName("PriceAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
                
            priceBuilder.Property(m => m.Currency)
                .HasColumnName("PriceCurrency")
                .HasMaxLength(3)
                .IsRequired()
                .HasDefaultValue("USD");
        });
        
        // Indexes for performance
        builder.HasIndex(ci => ci.CartId);
        builder.HasIndex(ci => ci.ProductId);
        builder.HasIndex(ci => new { ci.CartId, ci.ProductId })
            .IsUnique(); // Prevent duplicate items in same cart

        // Relations
        
        // A CartItem must belong to a Product.
        builder.HasOne(ci => ci.Product) 
            .WithMany()
            .HasForeignKey(ci => ci.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}