using BubbleSpaceApi.Core.Interfaces;

namespace BubbleSpaceApi.Core.Entities;

public class Profile : IBaseEntity<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Username { get; set; } = null!;
    public string Bio { get; set; } = null!;

    public IQueryable<Question> Questions { get; set; } = null!;
    public IQueryable<Answer> Answers { get; set; } = null!;

    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
}