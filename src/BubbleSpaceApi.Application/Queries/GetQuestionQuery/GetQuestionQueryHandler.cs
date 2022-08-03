using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Core.Interfaces;
using MediatR;

namespace BubbleSpaceApi.Application.Queries.GetQuestionQuery;

public class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, QuestionViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetQuestionQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<QuestionViewModel> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}