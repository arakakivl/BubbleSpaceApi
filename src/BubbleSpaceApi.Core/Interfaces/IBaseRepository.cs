namespace BubbleSpaceApi.Core.Interfaces;

public interface IBaseRepository<TKey, TEntity> where TEntity : class, IBaseEntity<TKey>
{
    Task<TKey> AddAsync(TEntity entity);

    Task<IQueryable<TEntity>> GetEntitiesAsync();
    Task<TEntity?> GetEntityAsync(TKey key);

    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TKey key);
}