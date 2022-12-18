using FluentValidation;

namespace BubbleSpaceApi.Application.Models.InputModels.LoginUserModel;

public class LoginUserValidator : AbstractValidator<LoginUserInputModel>
{
    // Error Messages
    private const string usernameOrEmailRequiredMessageError = "O nome de usuário ou e-mail não pode ser vazio.";
    private const string passwordRequiredMessageError = "A senha não pode ser vazia.";

    public LoginUserValidator()
    {
        RuleFor(x => x.UsernameOrEmail.Trim()).NotEmpty()
            .WithMessage(usernameOrEmailRequiredMessageError);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(passwordRequiredMessageError);
    }
}