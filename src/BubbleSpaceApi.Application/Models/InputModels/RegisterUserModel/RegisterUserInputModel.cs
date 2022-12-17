namespace BubbleSpaceApi.Application.Models.InputModels.RegisterUserModel;

public class RegisterUserInputModel : InputModelBase
{
    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public override bool IsValid() =>
        base.Validate(this, new RegisterUserValidator());
}