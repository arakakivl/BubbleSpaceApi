using MediatR;

namespace BubbleSpaceApi.Application.Commands.RegisterUserCommand;

public class RegisterUserCommand : IRequest<Unit>
{
    public string Username { get; }

    public string Email { get; }
    public string Password { get; }
    
    public RegisterUserCommand(string username, string email, string pswd)
    {
        Username = username;

        Email = email;
        Password = pswd;
    }
}