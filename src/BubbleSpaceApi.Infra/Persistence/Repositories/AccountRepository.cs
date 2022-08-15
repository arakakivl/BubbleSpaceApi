using BubbleSpaceApi.Core.Entities;
using BubbleSpaceApi.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BubbleSpaceApi.Infra.Persistence.Repositories;

public class AccountRepository : BaseRepository<Guid, Account>, IAccountRepository
{
    public AccountRepository(AppDbContext context) : base(context)
    {

    }

    public async Task<Account?> GetByEmailAsync(string email)
    {
        return await Task.FromResult(DbSet.Include(x => x.Profile).SingleOrDefault(x => x.Email == email));
    }
}