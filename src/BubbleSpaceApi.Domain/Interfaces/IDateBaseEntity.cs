namespace BubbleSpaceApi.Domain.Interfaces;

public interface IDateBaseEntity
{
    DateTimeOffset CreatedAt { get; set; }
}