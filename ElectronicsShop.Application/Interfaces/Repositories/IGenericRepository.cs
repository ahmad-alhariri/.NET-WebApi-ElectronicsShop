using System.Linq.Expressions;

namespace ElectronicsShop.Application.Interfaces.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(int id);
    IQueryable<TEntity> GetAll();
    IEnumerable<TEntity> GetAllAsNoTracking();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(List<TEntity> entities);

    void Update(TEntity entity);
    void UpdateRange(List<TEntity> entities);
    
    void Remove(TEntity entity);
    void RemoveRange(List<TEntity> entities);
    
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
}