using MediatR;

namespace BubbleSpaceApi.Application.Commands.DeleteQuestionCommand;

public class DeleteQuestionCommand : IRequest<Unit>
{
    public long QuestionId { get; set; }
    public Guid ProfileId { get; set; }

    public DeleteQuestionCommand(long id, Guid profileId)
    {
        QuestionId = id;
        ProfileId = profileId;
    }
}