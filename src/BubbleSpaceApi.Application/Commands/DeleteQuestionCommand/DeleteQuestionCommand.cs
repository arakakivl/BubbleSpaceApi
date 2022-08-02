using MediatR;

namespace BubbleSpaceApi.Application.Commands.DeleteQuestionCommand;

public class DeleteQuestionCommand : IRequest<Unit>
{
    public long Id { get; set; }
    public Guid ProfileId { get; set; }

    public DeleteQuestionCommand(long id, Guid profileId)
    {
        Id = id;
        ProfileId = profileId;
    }
}