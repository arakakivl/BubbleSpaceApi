using BubbleSpaceApi.Core.Entities;
using BubbleSpaceApi.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BubbleSpaceApi.Infra.Persistence.Repositories;

public class ProfileRepository : BaseRepository<Guid, Profile>, IProfileRepository
{
    public ProfileRepository(AppDbContext context) : base(context)
    {

    }

    public async Task<Profile?> GetByUsernameAsync(string username)
    {
        return await Task.FromResult(DbSet.Include(x => x.Account).Include(x => x.Questions).Include(x => x.Answers).SingleOrDefault(x => x.Username.ToLower() == username.ToLower()));
    }
}