using BubbleSpaceApi.Domain.Entities;

namespace BubbleSpaceApi.Domain.Interfaces.Repositories;

public interface IAccountRepository : IBaseRepository<Guid, Account>
{
    Task <Account?> GetByEmailAsync(string email);
}