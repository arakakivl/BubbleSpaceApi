using BubbleSpaceApi.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BubbleSpaceApi.Infra.Persistence.Repositories;

public class BaseRepository<TKey, TEntity> : IBaseRepository<TKey, TEntity> where TEntity : class, IBaseEntity<TKey>
{
    private readonly DbSet<TEntity> _dbSet;
    public DbSet<TEntity> DbSet => _dbSet;

    public BaseRepository(AppDbContext context)
    {
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TKey> AddAsync(TEntity entity)
    {
        _dbSet.Add(entity);
        return await Task.FromResult(entity.Id);
    }

    public async Task<ICollection<TEntity>> GetEntitiesAsync()
    {
        return await Task.FromResult(_dbSet.ToList());
    }

    public async Task<TEntity?> GetEntityAsync(TKey key)
    {
        return await _dbSet.FindAsync(key);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(TKey key)
    {
        var entity = await _dbSet.FindAsync(key);
        _dbSet.Remove(entity!);
    }
}