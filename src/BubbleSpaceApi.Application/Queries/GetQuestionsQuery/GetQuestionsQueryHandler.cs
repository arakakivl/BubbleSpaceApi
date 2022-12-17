using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Domain.Interfaces;
using BubbleSpaceApi.Application.Common;
using MediatR;

namespace BubbleSpaceApi.Application.Queries.GetQuestionsQuery;

public class GetQuestionsQueryHandler : IRequestHandler<GetQuestionsQuery, ICollection<QuestionViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetQuestionsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ICollection<QuestionViewModel>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
    {
        var questions = (await _unitOfWork.QuestionRepository.GetEntitiesAsync(null, "Profile,Answers"));

        // Inserting each profile into each answer in each question. This is neccessary because we need to return it as ViewModel.
        foreach(var q in questions)
            foreach(var a in q.Answers)
                a.Profile = (await _unitOfWork.ProfileRepository.GetEntityAsync(a.ProfileId))!;

        return questions.Select(x => x.AsViewModel()).ToList();
    }
}