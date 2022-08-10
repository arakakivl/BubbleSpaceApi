using MediatR;
using Microsoft.AspNetCore.Mvc;
using BubbleSpaceApi.Application.Models.InputModels.RegisterUserModel;
using BubbleSpaceApi.Application.Models.InputModels.LoginUserInputModel;
using BubbleSpaceApi.Application.Commands.RegisterUserCommand;
using BubbleSpaceApi.Api.Auth;
using BubbleSpaceApi.Application.Commands.LoginUserCommand;
using Microsoft.AspNetCore.Authorization;

namespace BubbleSpaceApi.Api.Controllers;

[Route("/auth")]
[ApiController]
[AllowAnonymous]
public class AccountController : ControllerBase
{
    private readonly ISender _sender;
    public AccountController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpPost("signup")]
    public async Task<IActionResult> SignUpAsync([FromBody] RegisterUserInputModel model)
    {
        if (HttpContext.Request.Cookies.SingleOrDefault(x => x.Key == "bsacc").Value is not null || HttpContext.Request.Cookies.SingleOrDefault(x => x.Key == "bsrfh").Value is not null)
            return BadRequest("Você já está autenticado.");

        var cmd = new RegisterUserCommand(model.Username, model.Email, model.Password);
        await _sender.Send(cmd);

        return Ok();
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignInAsync([FromBody] LoginUserInputModel model)
    {

        if (HttpContext.Request.Cookies.SingleOrDefault(x => x.Key == "bsacc").Value is not null || HttpContext.Request.Cookies.SingleOrDefault(x => x.Key == "bsrfh").Value is not null)
            return BadRequest("Você já está autenticado.");

        var cmd = new LoginUserCommand(model.UsernameOrEmail, model.Password);
        var profileId = await _sender.Send(cmd);

        if (profileId.Equals(Guid.Empty))
            return BadRequest("Usuário, email ou senha inválidos.");
        
        // Cookies config
        HttpContext.Response.Cookies.Append("bsacc", BubbleSpaceApi.Api.Auth.Auth.GenerateJwtToken(profileId), new CookieOptions() 
        {
            HttpOnly = false
        });

        HttpContext.Response.Cookies.Append("bsrfh", BubbleSpaceApi.Api.Auth.Auth.GenerateRefreshToken(), new CookieOptions() 
        {
            HttpOnly = true
        });

        return Ok();
    }
}