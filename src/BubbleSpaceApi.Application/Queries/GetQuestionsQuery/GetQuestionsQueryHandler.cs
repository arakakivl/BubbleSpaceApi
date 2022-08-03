using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Core.Interfaces;
using BubbleSpaceAPi.Application.Common;
using MediatR;

namespace BubbleSpaceApi.Application.Queries.GetQuestionsQuery;

public class GetQuestionsQueryHandler : IRequestHandler<GetQuestionsQuery, IQueryable<QuestionViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetQuestionsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IQueryable<QuestionViewModel>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
    {
        // TODO: get questions in function of the last id and the
        return (await _unitOfWork.QuestionRepository.GetEntitiesAsync()).Select(x => x.AsViewModel());
    }
}