using BubbleSpaceApi.Domain.Interfaces;
using MediatR;
using BubbleSpaceApi.Application.Common;
using BubbleSpaceApi.Core.Communication.Mediator;

namespace BubbleSpaceApi.Application.Commands.LoginUserCommand;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediatorHandler _mediatorHandler;

    public LoginUserCommandHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
    {
        _unitOfWork = unitOfWork;
        _mediatorHandler = mediatorHandler;
    }
    
    public async Task<Guid> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var usernameAcc = (await _unitOfWork.ProfileRepository.GetByUsernameAsync(request.UsernameOrEmail));
        var emailAcc = await _unitOfWork.AccountRepository.GetByEmailAsync(request.UsernameOrEmail);

        if (usernameAcc is null && emailAcc is null)
            return Guid.Empty;

        if (usernameAcc is null)
            if (PasswordHashing.VerifyHashes(request.Password, emailAcc!.PasswordHash))
                return emailAcc.Profile.Id;
            else
                return Guid.Empty;
        else
            if (PasswordHashing.VerifyHashes(request.Password, usernameAcc.Account.PasswordHash))
                return usernameAcc.Id;
            else
                return Guid.Empty;
    }
}