using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Core.Interfaces;
using BubbleSpaceAPi.Application.Common;
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
        return (await _unitOfWork.QuestionRepository.GetEntitiesAsync(null, "Profile,Answers")).Select(x => x.AsViewModel()).ToList();
    }
}