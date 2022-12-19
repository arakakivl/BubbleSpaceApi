using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Domain.Interfaces;
using BubbleSpaceApi.Application.Common;
using MediatR;
using BubbleSpaceApi.Core.Communication.Mediator;

namespace BubbleSpaceApi.Application.Queries.GetQuestionQuery;

public class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, QuestionViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediatorHandler _mediatorHandler;

    public GetQuestionQueryHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
    {
        _mediatorHandler = mediatorHandler;
        _unitOfWork = unitOfWork;
    }

    public async Task<QuestionViewModel> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
    {
        var question = (await _unitOfWork.QuestionRepository.GetEntitiesAsync(x => x.Id == request.QuestionId, "Profile,Answers")).SingleOrDefault();
        if (question is null)
            throw new EntityNotFoundException("Pergunta não encontrada.");
        
        // Inserting each profile into each answer. This is neccessary because we need to return it as ViewModel.
        foreach(var a in question.Answers)
            a.Profile = (await _unitOfWork.ProfileRepository.GetEntityAsync(a.ProfileId))!;

        return question.AsViewModel();
    }
}