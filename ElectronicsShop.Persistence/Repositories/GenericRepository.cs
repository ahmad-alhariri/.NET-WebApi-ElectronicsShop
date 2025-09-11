using System.Linq.Expressions;
using ElectronicsShop.Application.Interfaces.Repositories;
using ElectronicsShop.Persistence.DataContext;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Persistence.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbSet.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbSet;
    }

    public IEnumerable<TEntity> GetAllAsNoTracking()
    {
        return _dbSet.AsNoTracking().AsEnumerable();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(List<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void UpdateRange(List<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(List<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public IQueryable<TEntity> GetAllAsync()
    {
        return _dbSet.AsNoTracking().AsQueryable();
    }

    public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).AsNoTracking().ToListAsync().ConfigureAwait(false);
    }
    
    

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate).ConfigureAwait(false);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return predicate == null
            ? await _dbSet.CountAsync().ConfigureAwait(false)
            : await _dbSet.CountAsync(predicate).ConfigureAwait(false);
    }


}