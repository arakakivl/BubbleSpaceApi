using BubbleSpaceApi.Core.Entities;
using BubbleSpaceApi.Core.Interfaces.Repositories;

namespace BubbleSpaceApi.Infra.Persistence.Repositories;

public class AnswerRepository : BaseRepository<Guid, Answer>, IAnswerRepository
{
    public AnswerRepository(AppDbContext context) : base(context)
    {

    }
}