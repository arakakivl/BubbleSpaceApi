using MediatR;

namespace BubbleSpaceApi.Application.Commands.RegisterUserCommand;

public class RegisterUserCommand : IRequest<Unit>
{
    public string Username { get; set; }

    public string Email { get; set; }
    public string Password { get; set; }
    
    public RegisterUserCommand(string username, string email, string pswd)
    {
        Username = username;

        Email = email;
        Password = pswd;
    }
}