using System.Security.Cryptography;
using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using MediatR;
using BubbleSpaceApi.Application.Common;

namespace BubbleSpaceApi.Application.Commands.RegisterUserCommand;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    public RegisterUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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