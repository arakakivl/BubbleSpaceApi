using BubbleSpaceApi.Core.Interfaces;
using MediatR;

namespace BubbleSpaceApi.Application.Commands.LoginUserCommand;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public LoginUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public Task<bool> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}