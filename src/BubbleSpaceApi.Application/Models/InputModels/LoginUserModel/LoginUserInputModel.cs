using MediatR;

namespace BubbleSpaceApi.Application.Models.InputModels.LoginUserInputModel;

public class LoginUserInputModel
{
    public string UsernameOrEmail { get; set; } = null!;
    public string Password { get; set; } = null!;
}