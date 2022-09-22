using BubbleSpaceApi.Core.Interfaces;
using BubbleSpaceApi.Core.Interfaces.Repositories;
using BubbleSpaceApi.Infra.Persistence;
using BubbleSpaceApi.Infra.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BubbleSpaceApi.Infra;

public static class AddServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection collection)
    {
        collection.AddDbContext<AppDbContext>(x => x.UseInMemoryDatabase("db"));

        collection.AddTransient<IAccountRepository, AccountRepository>();
        collection.AddTransient<IProfileRepository, ProfileRepository>();
        collection.AddTransient<IQuestionRepository, QuestionRepository>();
        collection.AddTransient<IAnswerRepository, AnswerRepository>();

        collection.AddTransient<IUnitOfWork, UnitOfWork>();

        return collection;
    }
}