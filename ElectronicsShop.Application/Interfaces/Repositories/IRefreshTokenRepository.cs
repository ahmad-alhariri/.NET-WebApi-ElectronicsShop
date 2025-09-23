
using ElectronicsShop.Domain.Users.Identity;

namespace ElectronicsShop.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository:IGenericRepository<RefreshToken>
{
    Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId);
    Task<RefreshToken?> GetByTokenStringForUserAsync(string tokenString, Guid userId);
    Task<RefreshToken?> GetByTokenStringAsync(string tokenString);
}