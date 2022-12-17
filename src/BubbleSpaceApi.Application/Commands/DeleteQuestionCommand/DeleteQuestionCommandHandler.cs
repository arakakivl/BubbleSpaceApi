using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Domain.Interfaces;
using MediatR;

namespace BubbleSpaceApi.Application.Commands.DeleteQuestionCommand;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    public DeleteQuestionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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