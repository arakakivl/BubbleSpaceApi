using MediatR;

namespace BubbleSpaceApi.Application.Commands.DeleteQuestionCommand;

public class DeleteQuestionCommand : IRequest<Unit>
{
    public long Id { get; set; }
    public DeleteQuestionCommand(long id)
    {
        Id = id;
    }
}