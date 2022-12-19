using BubbleSpaceApi.Core.Communication.Mediator;
using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using MediatR;

namespace BubbleSpaceApi.Application.Commands.AskQuestionCommand;

public class AskQuestionCommandHandler : IRequestHandler<AskQuestionCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediatorHandler _mediatorHandler;
    
    public AskQuestionCommandHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
    {
        _unitOfWork = unitOfWork;
        _mediatorHandler = mediatorHandler;
    }
    
    public async Task<long> Handle(AskQuestionCommand request, CancellationToken cancellationToken)
    {
        Question q = new()
        {
            Title = request.Title,
            Description = request.Description,
            ProfileId = request.ProfileId
        };

        // Auto generated id
        var id = (await _unitOfWork.QuestionRepository.AddAsync(q));
        await _unitOfWork.SaveChangesAsync();

        return id;
    }
}