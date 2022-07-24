using BubbleSpaceApi.Core.Interfaces;

namespace BubbleSpaceApi.Core.Entities;

public class Answer : IBaseEntity<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Text { get; set; } = null!;

    public long QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    public Guid ProfileId { get; set; }
    public Profile Profile { get; set; } = null!;
}