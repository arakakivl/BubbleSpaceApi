using MediatR;

namespace BubbleSpaceApi.Application.Commands.AnswerQuestionCommand;

public class AnswerQuestionCommand : IRequest<Unit>
{
    public long QuestionId { get; set; }
    public Guid ProfileId { get; set; }

    public string Text { get; set; }

    public AnswerQuestionCommand(long questionId, Guid profileId, string text)
    {
        QuestionId = questionId;
        ProfileId = profileId;

        Text = text;
    }
}