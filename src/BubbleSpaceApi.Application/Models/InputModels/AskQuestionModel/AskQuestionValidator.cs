using BubbleSpaceApi.Domain.Interfaces;
using FluentValidation;

namespace BubbleSpaceApi.Application.Models.InputModels.AskQuestionModel;

public class AskQuestionValidator : AbstractValidator<AskQuestionInputModel>
{
    // Error Messages
    private const string questionTitleLengthMessageError = "O título de sua pergunta deve ter entre um e cem caracteres.";
    private const string questionDescriptionLengthMessageError = "A descrição de sua pergunta deve ter entre zero e dois mil caracteres.";

    // Some config
    private const int minimumLengthForQuestionTitle = 1;
    private const int maximumLengthForQuestionTitle = 100;
    
    private const int maximumLengthForQuestionDesc = 2000;

    public AskQuestionValidator()
    {
        RuleFor(q => q.Title).MaximumLength(maximumLengthForQuestionTitle)
            .WithMessage(questionTitleLengthMessageError);

        RuleFor(q => q.Title).MinimumLength(minimumLengthForQuestionTitle)
            .WithMessage(questionTitleLengthMessageError);

        RuleFor(q => q.Description).MaximumLength(maximumLengthForQuestionDesc)
            .WithMessage(questionDescriptionLengthMessageError);
    }

}