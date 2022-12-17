using System.Text;
using BubbleSpaceApi.Application.Models.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BubbleSpaceApi.Application.Models.InputModels;

public abstract class InputModelBase
{
    public List<string> Errors = new();

    public bool Validate<T, V>(T model, V validator) where V : AbstractValidator<T>
    {
        var result = validator.Validate(model);
        if (!result.IsValid)
        {
            foreach(var e in result.Errors)
                Errors.Add($"{e.PropertyName}:{e.ErrorMessage}");
        }

        return result.IsValid;
    }

    public UnprocessableEntityObjectResult ReturnUnprocessableEntity() =>
        new UnprocessableEntityObjectResult(new ResultViewModel("Requisição inválida.", false, Errors));

    public abstract bool IsValid();
}