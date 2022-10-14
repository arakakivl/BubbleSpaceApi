using BubbleSpaceApi.Core.Interfaces;

namespace BubbleSpaceApi.Core.Entities;

public class Account : BaseEntity<Guid>
{
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public Profile Profile { get; set; } = null!;

    public Account() : base(Guid.NewGuid())
    {
        
    }
}