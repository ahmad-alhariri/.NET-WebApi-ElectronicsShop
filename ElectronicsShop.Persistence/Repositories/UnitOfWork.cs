using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Persistence.DataContext;
using Microsoft.EntityFrameworkCore.Storage;

namespace ElectronicsShop.Persistence.Repositories;

public class UnitOfWork:IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IDbContextTransaction> BeginTransactionAsync() => _dbContext.Database.BeginTransactionAsync();
    
    public Task CommitAsync() => _dbContext.Database.CommitTransactionAsync();
    
    public Task RollbackAsync() => _dbContext.Database.RollbackTransactionAsync();
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>  _dbContext.SaveChangesAsync(cancellationToken);
    
    public ValueTask DisposeAsync() => _dbContext.DisposeAsync();
}