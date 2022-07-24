using BubbleSpaceApi.Core.Entities;

namespace BubbleSpaceApi.Core.Interfaces.Repositories;

public interface IAccountRepository : IBaseRepository<Guid, Account>
{
    Task <Account?> GetByEmailAsync(string email);
}