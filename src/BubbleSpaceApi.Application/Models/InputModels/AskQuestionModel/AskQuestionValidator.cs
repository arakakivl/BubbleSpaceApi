using BubbleSpaceApi.Core.Interfaces;
using FluentValidation;

namespace BubbleSpaceApi.Application.Models.InputModels.AskQuestionModel;

public class AskQuestionValidator : AbstractValidator<AskQuestionInputModel>
{
    private readonly IUnitOfWork _unitOfWork;
    public AskQuestionValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(q => q.Title).MaximumLength(100)
            .WithMessage("O título de sua pergunta deve ter entre um e cem caracteres.");

        RuleFor(q => q.Description).MaximumLength(2000)
            .WithMessage("O título de sua pergunta deve ter entre zero e dois mil caracteres.");
    }

}