using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces.Repositories;

namespace BubbleSpaceApi.Infra.Persistence.Repositories;

public class QuestionRepository : BaseRepository<long, Question>, IQuestionRepository
{
    public QuestionRepository(AppDbContext context) : base(context)
    {

    }
}