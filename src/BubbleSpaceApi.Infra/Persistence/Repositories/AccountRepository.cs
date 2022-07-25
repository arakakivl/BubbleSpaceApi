using BubbleSpaceApi.Core.Entities;
using BubbleSpaceApi.Core.Interfaces.Repositories;

namespace BubbleSpaceApi.Infra.Persistence.Repositories;

public class AccountRepository : BaseRepository<Guid, Account>, IAccountRepository
{
    public AccountRepository(AppDbContext context) : base(context)
    {

    }

    public async Task<Account?> GetByEmailAsync(string email)
    {
        return await Task.FromResult(DbSet.SingleOrDefault(x => x.Email == email));
    }
}