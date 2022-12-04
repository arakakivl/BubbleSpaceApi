using System.Linq.Expressions;
using BubbleSpaceApi.Core.Entities;

namespace BubbleSpaceApi.Core.Interfaces;

public interface IBaseRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>
{
    Task<TKey> AddAsync(TEntity entity);

    Task<IQueryable<TEntity>> GetEntitiesAsync();
    Task<TEntity?> GetEntityAsync(TKey key);

    Task<IQueryable<TEntity>> GetEntitiesAsync(Expression<Func<TEntity, bool>>? filter = null, string includeProperties = "");

    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TKey key);
}