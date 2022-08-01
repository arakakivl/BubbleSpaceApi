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

    public Task<Unit> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}