using FluentValidation;

namespace BubbleSpaceApi.Application.Models.InputModels.AnswerQuestionModel;

public class AnswerQuestionValidator : AbstractValidator<AnswerQuestionInputModel>
{
    // Error messages
    private const string exceededAnswerLengthMessage = "Resposta grande demais! Foram dois mil caracteres!!!";

    //  Some config
    private const int answerMaximumLength = 2000;

    public AnswerQuestionValidator()
    {
        RuleFor(ans => ans.Text).MaximumLength(answerMaximumLength)
            .WithMessage(exceededAnswerLengthMessage);
    }
}