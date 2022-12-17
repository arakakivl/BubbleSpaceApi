namespace BubbleSpaceApi.Application.Models.InputModels.AnswerQuestionModel;

public class AnswerQuestionInputModel : InputModelBase
{
    public string Text { get; set; } = null!;

    public override bool IsValid() =>
        base.Validate(this, new AnswerQuestionValidator());
}