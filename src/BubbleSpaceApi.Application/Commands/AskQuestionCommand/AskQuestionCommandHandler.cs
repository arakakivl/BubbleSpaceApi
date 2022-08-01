using BubbleSpaceApi.Core.Interfaces;
using MediatR;

namespace BubbleSpaceApi.Application.Commands.AskQuestionCommand;

public class AskQuestionCommandHandler : IRequestHandler<AskQuestionCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;
    public AskQuestionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public Task<long> Handle(AskQuestionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}