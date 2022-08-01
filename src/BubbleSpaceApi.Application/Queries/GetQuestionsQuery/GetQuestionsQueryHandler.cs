using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Core.Interfaces;
using MediatR;

namespace BubbleSpaceApi.Application.Queries.GetQuestionsQuery;

public class GetQuestionsQueryHandler : IRequestHandler<GetQuestionsQuery, IQueryable<QuestionViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetQuestionsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<IQueryable<QuestionViewModel>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}