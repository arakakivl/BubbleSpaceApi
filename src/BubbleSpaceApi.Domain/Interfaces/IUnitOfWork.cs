using BubbleSpaceApi.Domain.Interfaces.Repositories;

namespace BubbleSpaceApi.Domain.Interfaces;

public interface IUnitOfWork
{
    IAccountRepository AccountRepository { get; }
    IProfileRepository ProfileRepository { get; }
    IQuestionRepository QuestionRepository { get; }
    IAnswerRepository AnswerRepository { get; }

    Task SaveChangesAsync();
}