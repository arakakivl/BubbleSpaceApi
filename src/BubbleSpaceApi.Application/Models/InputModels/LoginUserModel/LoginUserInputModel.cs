using MediatR;

namespace BubbleSpaceApi.Application.Models.InputModels.LoginUserModel;

public class LoginUserInputModel : InputModelBase
{
    public string UsernameOrEmail { get; set; } = null!;
    public string Password { get; set; } = null!;

    public override bool IsValid() =>
        base.Validate(this, new LoginUserValidator());
}