using ElectronicsShop.Domain.Carts;
using ElectronicsShop.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicsShop.Persistence.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        // Primary Key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.Id)
            .ValueGeneratedNever(); // We generate GUIDs in domain

        builder.Property(c => c.UserId)
            .IsRequired(false);

        // Indexes for performance
        builder.HasIndex(c => c.UserId)
            .HasFilter("[UserId] IS NOT NULL");

        // Relations
        // 1. Cart -> User (Optional: for registered users)
        builder.HasOne<User>()
            .WithMany() 
            .HasForeignKey(c => c.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade); 

        // 2. Cart -> CartItems (One-to-Many)
        builder.HasMany(c => c.Items)
            .WithOne(ci => ci.Cart)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade); 

    }
}