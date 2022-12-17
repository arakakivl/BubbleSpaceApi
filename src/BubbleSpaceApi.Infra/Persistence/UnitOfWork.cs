using BubbleSpaceApi.Domain.Interfaces;
using BubbleSpaceApi.Domain.Interfaces.Repositories;
using BubbleSpaceApi.Infra.Persistence.Repositories;

namespace BubbleSpaceApi.Infra.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IAccountRepository AccountRepository { get; set; }
    public IProfileRepository ProfileRepository { get; set; }
    public IQuestionRepository QuestionRepository { get; set; }
    public IAnswerRepository AnswerRepository { get; set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;

        AccountRepository ??= new AccountRepository(context);
        ProfileRepository ??= new ProfileRepository(context);
        QuestionRepository ??= new QuestionRepository(context);
        AnswerRepository ??= new AnswerRepository(context);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}