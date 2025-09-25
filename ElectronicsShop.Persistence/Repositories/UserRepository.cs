using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Domain.Users;
using ElectronicsShop.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;


namespace ElectronicsShop.Persistence.Repositories;

public class UserRepository: GenericRepository<User>,IUserRepository
{
    private readonly DbSet<User> _users;
    
    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _users = context.Set<User>();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _users.FirstOrDefaultAsync(u => u.Id == id);
    }
}