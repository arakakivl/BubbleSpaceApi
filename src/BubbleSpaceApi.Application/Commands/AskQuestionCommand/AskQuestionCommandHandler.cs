using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using MediatR;

namespace BubbleSpaceApi.Application.Commands.AskQuestionCommand;

public class AskQuestionCommandHandler : IRequestHandler<AskQuestionCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;
    public AskQuestionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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