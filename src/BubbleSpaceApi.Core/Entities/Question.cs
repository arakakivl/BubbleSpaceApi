using BubbleSpaceApi.Core.Interfaces;

namespace BubbleSpaceApi.Core.Entities;

public class Question : BaseEntity<long>
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = "";

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public Guid ProfileId { get; set; }
    public virtual Profile Profile { get; set; } = null!;

    public Question() : base(0)
    {
        
    }

    public bool UserAnswered(Guid profId) =>
        Answers.Any(x => x.ProfileId == profId);

    public bool UserOwnsQuestion(Guid profId) =>
        ProfileId == profId;
}