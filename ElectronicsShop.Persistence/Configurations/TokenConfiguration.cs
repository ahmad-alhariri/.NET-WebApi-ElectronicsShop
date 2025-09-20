using ElectronicsShop.Domain.Users.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicsShop.Persistence.Configurations;

public class TokenConfiguration: IEntityTypeConfiguration<RefreshToken>
{
    
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        
        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Token).IsRequired();
    }
}