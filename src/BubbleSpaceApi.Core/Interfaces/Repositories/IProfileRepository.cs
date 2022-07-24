using BubbleSpaceApi.Core.Entities;

namespace BubbleSpaceApi.Core.Interfaces.Repositories;

public interface IProfileRepository : IBaseRepository<Guid, Profile>
{
    Task<Profile?> GetByUsernameAsync(string username);
}