using BubbleSpaceApi.Core.Interfaces;

namespace BubbleSpaceApi.Core.Entities;

public class Question : IBaseEntity<long>
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;
    public string Description { get; set; } = "";

    public IQueryable<Answer> Answers { get; set; } = null!;

    public Guid ProfileId { get; set; }
    public Profile Profile { get; set; } = null!;
}