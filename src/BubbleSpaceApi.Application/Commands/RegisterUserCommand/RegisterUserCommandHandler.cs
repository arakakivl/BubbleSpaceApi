using System.Security.Cryptography;
using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using MediatR;
using BubbleSpaceApi.Application.Common;
using BubbleSpaceApi.Core.Communication.Mediator;

namespace BubbleSpaceApi.Application.Commands.RegisterUserCommand;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediatorHandler _mediatorHandler;

    public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IMediatorHandler mediatorHandler)
    {
        _unitOfWork = unitOfWork;
        _mediatorHandler = mediatorHandler;
    }
    
    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var account = new Account()
        {
            Email = request.Email,
            PasswordHash = PasswordHashing.GeneratePasswordHash(request.Password)
        };

        var profile = new Profile()
        {
            AccountId = account.Id,
            Username = request.Username.Trim().Replace("  ", " "),
        };

        await _unitOfWork.AccountRepository.AddAsync(account);
        await _unitOfWork.ProfileRepository.AddAsync(profile);

        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}