using BubbleSpaceApi.Domain.Interfaces;

namespace BubbleSpaceApi.Domain.Entities;

public class Account : BaseEntity<Guid>
{
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public virtual Profile Profile { get; set; } = null!;

    public Account() : base(Guid.NewGuid())
    {
        
    }
}