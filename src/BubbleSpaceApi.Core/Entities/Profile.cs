using BubbleSpaceApi.Core.Interfaces;

namespace BubbleSpaceApi.Core.Entities;

public class Profile : IBaseEntity<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Username { get; set; } = null!;
    public string Bio { get; set; } = "";

    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
}