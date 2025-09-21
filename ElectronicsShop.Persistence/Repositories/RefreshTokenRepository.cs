using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Domain.Users.Identity;
using ElectronicsShop.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Persistence.Repositories;

public class RefreshTokenRepository:GenericRepository<RefreshToken>, IRefreshTokenRepository
{
    private readonly DbSet<RefreshToken> _refreshTokens;
    
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
        _refreshTokens = context.Set<RefreshToken>();
    }

    public async Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId)
    {
        return await _refreshTokens
            .Where(rt => rt.UserId == userId &&
                         rt.RevokedOnUtc == null &&              
                         rt.ExpiresOnUtc > DateTimeOffset.UtcNow) 
            .ToListAsync();
    }
}