using MediatR;

namespace BubbleSpaceApi.Application.Commands.DeleteAnswerCommand;

public class DeleteAnswerCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public DeleteAnswerCommand(Guid id)
    {
        Id = id;
    }
}