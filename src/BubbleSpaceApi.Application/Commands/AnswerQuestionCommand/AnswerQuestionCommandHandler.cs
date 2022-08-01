using BubbleSpaceApi.Core.Interfaces;
using MediatR;

namespace BubbleSpaceApi.Application.Commands.AnswerQuestionCommand;

public class AnswerQuestionCommandHandler : IRequestHandler<AnswerQuestionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    public AnswerQuestionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<Unit> Handle(AnswerQuestionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}