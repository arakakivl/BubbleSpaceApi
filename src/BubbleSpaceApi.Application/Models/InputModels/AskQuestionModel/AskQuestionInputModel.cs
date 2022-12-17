namespace BubbleSpaceApi.Application.Models.InputModels.AskQuestionModel;

public class AskQuestionInputModel : InputModelBase
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = "";

    public override bool IsValid() =>
        base.Validate(this, new AskQuestionValidator());
}