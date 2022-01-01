namespace APITest.Repositories;

using Microsoft.EntityFrameworkCore;
using Models;

public interface IBaseRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();
    Task<TEntity> CreateAndSaveAsync(TEntity entity);
    Task<TEntity> UpdateAndSaveAsync(TEntity entity);
    Task DeleteAndSaveAsync(TEntity entity);
}

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    private readonly TestDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public BaseRepository(TestDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<TEntity> CreateAndSaveAsync(TEntity entity)
    {
        var created = await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();

        return created.Entity;
    }

    public async Task<TEntity> UpdateAndSaveAsync(TEntity entity)
    {
        var updated = _dbSet.Update(entity);
        await _context.SaveChangesAsync();

        return updated.Entity;
    }

    public async Task DeleteAndSaveAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}