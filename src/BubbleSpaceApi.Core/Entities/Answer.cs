using BubbleSpaceApi.Core.Interfaces;

namespace BubbleSpaceApi.Core.Entities;

public class Answer : BaseEntity<Guid>
{
    public string Text { get; set; } = null!;

    public long QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    public Guid ProfileId { get; set; }
    public Profile Profile { get; set; } = null!;

    public Answer() : base(Guid.NewGuid())
    {
        
    }
}