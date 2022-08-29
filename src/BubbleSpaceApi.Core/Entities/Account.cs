using BubbleSpaceApi.Core.Interfaces;

namespace BubbleSpaceApi.Core.Entities;

public class Account : IBaseEntity<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public Profile Profile { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}