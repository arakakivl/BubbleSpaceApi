using BubbleSpaceApi.Core.Interfaces;
using MediatR;
using BubbleSpaceApi.Application.Common;

namespace BubbleSpaceApi.Application.Commands.LoginUserCommand;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public LoginUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<bool> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var usernameAcc = (await _unitOfWork.ProfileRepository.GetByUsernameAsync(request.UsernameOrEmail));
        var emailAcc = await _unitOfWork.AccountRepository.GetByEmailAsync(request.UsernameOrEmail);

        if (usernameAcc is null && emailAcc is null)
            return false;

        if (usernameAcc is null)
            return PasswordHashing.VerifyHashes(request.Password, emailAcc!.PasswordHash);
        else
            return PasswordHashing.VerifyHashes(request.Password, usernameAcc.Account.PasswordHash);
    }
}