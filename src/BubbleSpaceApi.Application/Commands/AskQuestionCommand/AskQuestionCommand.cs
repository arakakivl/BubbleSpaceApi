using MediatR;

namespace BubbleSpaceApi.Application.Commands.AskQuestionCommand;

public class AskQuestionCommand : IRequest<long>
{
    public Guid ProfileId { get; }

    public string Title { get; }
    public string Description { get; } = "";

    public AskQuestionCommand(Guid profileId, string title, string description = "")
    {
        ProfileId = profileId;

        Title = title.EndsWith("?") ? title.Trim() : title.Trim() + "?";
        Description = description.Trim();
    }
}