using BubbleSpaceApi.Core.Interfaces;

namespace BubbleSpaceApi.Core.Entities;

public class BaseEntity<TKey> : IBaseEntity<TKey>
{
    public TKey Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public BaseEntity(TKey key)
    {
        Id = key;
    }
}