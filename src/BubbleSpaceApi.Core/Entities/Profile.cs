using BubbleSpaceApi.Core.Interfaces;

namespace BubbleSpaceApi.Core.Entities;

public class Profile : BaseEntity<Guid>
{
    public string Username { get; set; } = null!;
    public string Bio { get; set; } = "";

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public Guid AccountId { get; set; }
    public virtual Account Account { get; set; } = null!;

    public Profile() : base(Guid.NewGuid())
    {
        
    }
}