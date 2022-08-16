using BubbleSpaceApi.Application.Exceptions;
using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Core.Interfaces;
using BubbleSpaceAPi.Application.Common;
using MediatR;

namespace BubbleSpaceApi.Application.Queries.GetQuestionQuery;

public class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, QuestionViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetQuestionQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<QuestionViewModel> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
    {
        var question = (await _unitOfWork.QuestionRepository.GetEntitiesAsync(x => x.Id == request.Id, "Profile,Answers")).SingleOrDefault();
        if (question is null)
            throw new EntityNotFoundException("Pergunta não encontrada.");
        
        return question.AsViewModel();
    }
}