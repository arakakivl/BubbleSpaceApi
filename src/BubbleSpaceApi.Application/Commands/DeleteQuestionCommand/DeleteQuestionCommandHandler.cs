using BubbleSpaceApi.Core.Interfaces;
using MediatR;

namespace BubbleSpaceApi.Application.Commands.DeleteQuestionCommand;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    public DeleteQuestionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}