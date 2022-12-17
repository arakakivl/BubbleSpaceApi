using BubbleSpaceApi.Domain.Entities;

namespace BubbleSpaceApi.Domain.Interfaces.Repositories;

public interface IProfileRepository : IBaseRepository<Guid, Profile>
{
    Task<Profile?> GetByUsernameAsync(string username);
}