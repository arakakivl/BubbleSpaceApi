using BubbleSpaceApi.Domain.Interfaces;

namespace BubbleSpaceApi.Domain.Entities;

public class Answer : BaseEntity<Guid>
{
    public string Text { get; set; } = null!;

    public long QuestionId { get; set; }
    public virtual Question Question { get; set; } = null!;

    public Guid ProfileId { get; set; }
    public virtual Profile Profile { get; set; } = null!;

    public Answer() : base(Guid.NewGuid())
    {
        
    }
}