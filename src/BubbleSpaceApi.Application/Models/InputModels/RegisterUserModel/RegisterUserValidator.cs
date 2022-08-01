using BubbleSpaceApi.Core.Interfaces;
using FluentValidation;

namespace BubbleSpaceApi.Application.Models.InputModels.RegisterUserModel;

public class RegisterUserValidator : AbstractValidator<RegisterUserInputModel>
{
    private readonly IUnitOfWork _unitOfWork;
    public RegisterUserValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        // Username validation
        RuleFor(user => user.Username).Matches("")
            .WithMessage("O nome de usuário deve ter entre um e trinta caracteres e somente pode ter, entre letras e números, underlines, hífens e espaços em branco.");

        RuleFor(user => user).MustAsync(async (user, cancellation) => !(await UsernameAlreadyInUse(user.Username)))
            .WithMessage("O nome de usuário já está em uso.");

        // Email validation
        RuleFor(user => user.Email).Matches("^([a-zA-Z0-9]{1,}[\\.-_]?[a-zA-Z0-9]{1,})@[a-zA-Z]{2,15}[\\.][a-zA-Z]{2,10}([\\.][a-zA-Z]{2,5})?$")
            .WithMessage("Email inválido.");

        RuleFor(user => user).MustAsync(async (user, cancellation) => !(await EmailAlreadyInUse(user.Email)))
            .WithMessage("Este email já está cadastrado.");
    }

    private async Task<bool> UsernameAlreadyInUse(string username) =>
        (await _unitOfWork.ProfileRepository.GetByUsernameAsync(username)) != null;

    private async Task<bool> EmailAlreadyInUse(string email) =>
        (await _unitOfWork.AccountRepository.GetByEmailAsync(email)) != null;
}