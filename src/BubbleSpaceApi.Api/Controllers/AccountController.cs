using MediatR;
using Microsoft.AspNetCore.Mvc;
using BubbleSpaceApi.Application.Models.InputModels.RegisterUserModel;
using BubbleSpaceApi.Application.Models.InputModels.LoginUserInputModel;

namespace BubbleSpaceApi.Api.Controllers;

public class AccountController : BaseSenderController
{
    [HttpPost("signup")]
    public async Task<IActionResult> SignUpAsync([FromBody] RegisterUserInputModel model)
    {
        throw new NotImplementedException();
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignInAsync([FromBody] LoginUserInputModel model)
    {
        throw new NotImplementedException();
    }
}