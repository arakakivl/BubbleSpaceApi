using BubbleSpaceApi.Core.Interfaces;
using MediatR;

namespace BubbleSpaceApi.Application.Commands.RegisterUserCommand;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    public RegisterUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}