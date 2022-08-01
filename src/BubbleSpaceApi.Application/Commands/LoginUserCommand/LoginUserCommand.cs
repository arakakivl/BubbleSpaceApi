using MediatR;

namespace BubbleSpaceApi.Application.Commands.LoginUserCommand;

public class LoginUserCommand : IRequest<bool>
{
    public string UsernameOrEmail { get; }
    public string Password { get; }

    public LoginUserCommand(string usernameOrEmail, string password)
    {
        UsernameOrEmail = usernameOrEmail;
        Password = password;
    }
}