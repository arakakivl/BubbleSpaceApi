using BubbleSpaceApi.Core.Entities;
using BubbleSpaceApi.Core.Interfaces.Repositories;

namespace BubbleSpaceApi.Infra.Persistence.Repositories;

public class QuestionRepository : BaseRepository<long, Question>, IQuestionRepository
{
    public QuestionRepository(AppDbContext context) : base(context)
    {

    }
}