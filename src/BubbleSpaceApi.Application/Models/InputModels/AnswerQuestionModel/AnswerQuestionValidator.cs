using FluentValidation;

namespace BubbleSpaceApi.Application.Models.InputModels.AnswerQuestionModel;

public class AnswerQuestionValidator : AbstractValidator<AnswerQuestionInputModel>
{
    public AnswerQuestionValidator()
    {
        RuleFor(ans => ans.Text).MaximumLength(2000)
            .WithMessage("Resposta grande demais! Foram dois mil caracteres!!!");
    }
}