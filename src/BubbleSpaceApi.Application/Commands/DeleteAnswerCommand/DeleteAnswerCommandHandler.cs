using BubbleSpaceApi.Application.Exceptions;
using BubbleSpaceApi.Core.Interfaces;
using MediatR;

namespace BubbleSpaceApi.Application.Commands.DeleteAnswerCommand;

public class DeleteAnswerCommandHandler : IRequestHandler<DeleteAnswerCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    public DeleteAnswerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
    {
        var question = (await _unitOfWork.QuestionRepository.GetEntitiesAsync(q => q.Id == request.QuestionId, "Answers"))
            .SingleOrDefault();
        
        var answer = question?.Answers.SingleOrDefault(x => x.ProfileId == request.ProfileId);

        if (question is null)
            throw new EntityNotFoundException("Pergunta não encontrada.");
        else if (answer is null)
            throw new EntityNotFoundException("Resposta não encontrada.");

        await _unitOfWork.AnswerRepository.DeleteAsync(answer.Id);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}