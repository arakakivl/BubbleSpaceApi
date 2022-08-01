namespace BubbleSpaceApi.Application.Models.InputModels.RegisterUserModel;

public class RegisterUserInputModel
{
    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}