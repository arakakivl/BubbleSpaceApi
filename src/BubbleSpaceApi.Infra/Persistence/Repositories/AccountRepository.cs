using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BubbleSpaceApi.Infra.Persistence.Repositories;

public class AccountRepository : BaseRepository<Guid, Account>, IAccountRepository
{
    public AccountRepository(AppDbContext context) : base(context)
    {

    }

    public async Task<Account?> GetByEmailAsync(string email) =>
        (await this.GetEntitiesAsync(acc => acc.Email.ToLower() == email.ToLower(), "Profile")).SingleOrDefault();
}