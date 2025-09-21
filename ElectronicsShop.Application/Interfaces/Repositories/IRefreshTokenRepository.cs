
using ElectronicsShop.Domain.Users.Identity;

namespace ElectronicsShop.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository:IGenericRepository<RefreshToken>
{
    Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId);
}