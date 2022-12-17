namespace BubbleSpaceApi.Domain.Interfaces;

public interface IBaseEntity<TKey>
{
    TKey Id { get; set; }
}