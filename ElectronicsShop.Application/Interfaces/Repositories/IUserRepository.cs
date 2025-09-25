using ElectronicsShop.Domain.Users;

namespace ElectronicsShop.Application.Interfaces.Repositories;

public interface IUserRepository: IGenericRepository<User>
{
    Task<User?> GetUserByIdAsync(Guid id);
}