using BubbleSpaceApi.Core.Communication.Mediator;
using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Domain.Interfaces;
using MediatR;

namespace BubbleSpaceApi.Application.Commands.DeleteAnswerCommand;

public class DeleteAnswerCommandHandler : IRequestHandler<DeleteAnswerCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediatorHandler _mediatorHandler;
    
    public DeleteAnswerCommandHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
    {
        _unitOfWork = unitOfWork;
        _mediatorHandler = mediatorHandler;
    }

    public async Task<Unit> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
    {
        var question = (await _unitOfWork.QuestionRepository.GetEntitiesAsync(q => q.Id == request.QuestionId, "Answers"))
            .SingleOrDefault();
        
        var answer = question?.Answers.SingleOrDefault(x => x.ProfileId == request.ProfileId);

        if (question is null)
            throw new EntityNotFoundException("Pergunta não encontrada.");
        else if (!question.UserAnswered(request.ProfileId))
            throw new EntityNotFoundException("Resposta não encontrada.");

        // answer.Id never will be null if user answered the question
        await _unitOfWork.AnswerRepository.DeleteAsync(answer!.Id);
        
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}