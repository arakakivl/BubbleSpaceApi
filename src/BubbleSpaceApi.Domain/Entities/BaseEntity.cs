using BubbleSpaceApi.Domain.Interfaces;

namespace BubbleSpaceApi.Domain.Entities;

public class BaseEntity<TKey> : IBaseEntity<TKey>, IDateBaseEntity
{
    public TKey Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public BaseEntity(TKey key)
    {
        Id = key;
    }
}