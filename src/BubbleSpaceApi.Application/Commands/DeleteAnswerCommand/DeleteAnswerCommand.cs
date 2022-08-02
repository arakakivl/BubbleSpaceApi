using MediatR;

namespace BubbleSpaceApi.Application.Commands.DeleteAnswerCommand;

public class DeleteAnswerCommand : IRequest<Unit>
{
    public long QuestionId { get; set; }
    public Guid ProfileId { get; set; }
    public DeleteAnswerCommand(long questionId, Guid profileId)
    {
        QuestionId = questionId;
        ProfileId = profileId;
    }
}