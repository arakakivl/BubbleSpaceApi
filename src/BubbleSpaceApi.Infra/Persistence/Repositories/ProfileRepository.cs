using BubbleSpaceApi.Core.Entities;
using BubbleSpaceApi.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BubbleSpaceApi.Infra.Persistence.Repositories;

public class ProfileRepository : BaseRepository<Guid, Profile>, IProfileRepository
{
    public ProfileRepository(AppDbContext context) : base(context)
    {

    }

    public async Task<Profile?> GetByUsernameAsync(string username) =>
        (await this.GetEntitiesAsync(prof => prof.Username.ToLower() == username.ToLower(), "Account,Questions,Answers")).SingleOrDefault();
}