namespace BubbleSpaceApi.Core.Interfaces;

public interface IBaseEntity<TKey>
{
    TKey Id { get; set; }
    DateTimeOffset CreatedAt { get; set; }
}