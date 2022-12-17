using MediatR;

namespace BubbleSpaceApi.Application.Commands.LoginUserCommand;

public class LoginUserCommand : IRequest<Guid>
{
    public string UsernameOrEmail { get; set; }
    public string Password { get; set; }

    public LoginUserCommand(string usernameOrEmail, string password)
    {
        UsernameOrEmail = usernameOrEmail;
        Password = password;
    }
}