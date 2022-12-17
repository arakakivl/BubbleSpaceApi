using BubbleSpaceApi.Domain.Interfaces;
using FluentValidation;

namespace BubbleSpaceApi.Application.Models.InputModels.RegisterUserModel;

public class RegisterUserValidator : AbstractValidator<RegisterUserInputModel>
{
    // Error messages
    private const string invalidUsernameMessage = "O nome de usuário deve ter entre um e trinta caracteres e somente pode ter, entre letras e números, underlines, hífens e espaços em branco.";
    private const string usernameAlreadyInUseMessage = "O nome de usuário já está em uso.";

    private const string invalidEmailMessage = "Email inválido.";
    private const string emailAlreadyInUseMessage = "Este email já está cadastrado.";

    // Regex
    private const string validUsernameRegex = "^([a-zA-Z0-9_-]{1,}[ ]?[a-zA-Z0-9_ -]{1,}?)$)";
    private const string validEmailRegex = "^([a-zA-Z0-9]{1,}[\\.-_]?[a-zA-Z0-9]{1,})@[a-zA-Z]{2,15}[\\.][a-zA-Z]{2,10}([\\.][a-zA-Z]{2,5})?$";

    // Some config
    private const int maxUsernameLength = 30;

    public RegisterUserValidator()
    {
        // Username validation
        RuleFor(user => user.Username.Trim()).Matches("")
            .WithMessage(invalidUsernameMessage);

        RuleFor(user => user.Username).MaximumLength(maxUsernameLength)
            .WithMessage(invalidUsernameMessage);

        // Email validation
        RuleFor(user => user.Email).Matches(validEmailRegex)
            .WithMessage(invalidEmailMessage);
    }
}