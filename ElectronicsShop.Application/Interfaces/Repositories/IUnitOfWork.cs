using Microsoft.EntityFrameworkCore.Storage;

namespace ElectronicsShop.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}