using BubbleSpaceApi.Core.Communication.Mediator;
using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Domain.Interfaces;
using MediatR;

namespace BubbleSpaceApi.Application.Commands.DeleteQuestionCommand;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediatorHandler _mediatorHandler;

    public DeleteQuestionCommandHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
    {
        _unitOfWork = unitOfWork;
        _mediatorHandler = mediatorHandler;
    }
    
    public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.QuestionRepository.GetEntityAsync(request.QuestionId);
        if (question is null)
            throw new EntityNotFoundException("Pergunta não encontrada.");
        else if (!question.UserOwnsQuestion(request.ProfileId))
            throw new ForbiddenException("Ação não autorizada.");

        await _unitOfWork.QuestionRepository.DeleteAsync(request.QuestionId);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}