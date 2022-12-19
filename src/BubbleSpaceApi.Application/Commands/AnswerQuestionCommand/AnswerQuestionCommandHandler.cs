using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using MediatR;
using BubbleSpaceApi.Core.Communication.Mediator;
using BubbleSpaceApi.Core.Communication.Messages.Notifications;

namespace BubbleSpaceApi.Application.Commands.AnswerQuestionCommand;

public class AnswerQuestionCommandHandler : IRequestHandler<AnswerQuestionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediatorHandler _mediatorHandler;

    public AnswerQuestionCommandHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
    {
        _unitOfWork = unitOfWork;
        _mediatorHandler = mediatorHandler;
    }

    public async Task<Unit> Handle(AnswerQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = (await _unitOfWork.QuestionRepository.GetEntitiesAsync(q => q.Id == request.QuestionId, "Answers"))
            .SingleOrDefault();

        if (question is null)
            throw new EntityNotFoundException("Pergunta não encontrada.");
        else if (question.UserAnswered(request.ProfileId))
            throw new AlreadyAnsweredQuestionException("Pergunta já respondida.");
        
        var answer = new Answer()
        {
            ProfileId = request.ProfileId,
            QuestionId = request.QuestionId,
            Text = request.Text
        };

        await _unitOfWork.AnswerRepository.AddAsync(answer);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}