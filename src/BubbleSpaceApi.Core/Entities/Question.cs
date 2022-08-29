using BubbleSpaceApi.Core.Interfaces;

namespace BubbleSpaceApi.Core.Entities;

public class Question : IBaseEntity<long>
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;
    public string Description { get; set; } = "";

    public ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public Guid ProfileId { get; set; }
    public Profile Profile { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}