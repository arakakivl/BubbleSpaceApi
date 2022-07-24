using BubbleSpaceApi.Core.Interfaces.Repositories;

namespace BubbleSpaceApi.Core.Interfaces;

public interface IUnitOfWork
{
    IAccountRepository AccountRepository { get; }
    IProfileRepository ProfileRepository { get; }
    IQuestionRepository QuestionRepository { get; }
    IAnswerRepository AnswerRepository { get; }

    Task SaveChangesAsync();
}