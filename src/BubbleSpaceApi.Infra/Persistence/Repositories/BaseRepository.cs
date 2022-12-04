using System.Linq.Expressions;
using BubbleSpaceApi.Core.Entities;
using BubbleSpaceApi.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BubbleSpaceApi.Infra.Persistence.Repositories;

public class BaseRepository<TKey, TEntity> : IBaseRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>
{
    private readonly DbSet<TEntity> _dbSet;
    public DbSet<TEntity> DbSet => _dbSet;

    public BaseRepository(AppDbContext context)
    {
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<TKey> AddAsync(TEntity entity)
    {
        _dbSet.Add(entity);
        return await Task.FromResult(entity.Id);
    }

    public virtual async Task<IQueryable<TEntity>> GetEntitiesAsync() =>
        await Task.FromResult(_dbSet);

    public virtual async Task<IQueryable<TEntity>> GetEntitiesAsync(Expression<Func<TEntity, bool>>? filter = null, string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet;
        if (filter is not null)
            query = query.Where(filter);
        
        foreach(var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(includeProp);
        
        return await Task.FromResult(query);
    }

    public virtual async Task<TEntity?> GetEntityAsync(TKey key) =>
        await _dbSet.FindAsync(key);

    public virtual async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(TKey key)
    {
        var entity = await _dbSet.FindAsync(key);
        _dbSet.Remove(entity!);
    }
}