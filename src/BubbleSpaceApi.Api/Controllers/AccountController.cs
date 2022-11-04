using MediatR;
using Microsoft.AspNetCore.Mvc;
using BubbleSpaceApi.Application.Models.InputModels.RegisterUserModel;
using BubbleSpaceApi.Application.Models.InputModels.LoginUserInputModel;
using BubbleSpaceApi.Application.Commands.RegisterUserCommand;
using BubbleSpaceApi.Api.Auth;
using BubbleSpaceApi.Application.Commands.LoginUserCommand;
using Microsoft.AspNetCore.Authorization;

namespace BubbleSpaceApi.Api.Controllers;

[AllowAnonymous]
public class AccountController : ApiControllerBase
{
    private readonly IAuth _auth;
    public AccountController(ISender sender, IAuth auth) : base(sender)
    {
        _auth = auth;
    }
    
    [HttpPost("signup")]
    public async Task<IActionResult> SignUpAsync([FromBody] RegisterUserInputModel model)
    {
        if (HttpContext.Request.Cookies.SingleOrDefault(x => x.Key == "bsacc").Value is not null || HttpContext.Request.Cookies.SingleOrDefault(x => x.Key == "bsrfh").Value is not null)
            return BadRequest("Você já está autenticado.");

        var cmd = new RegisterUserCommand(model.Username, model.Email, model.Password);
        await Sender.Send(cmd);

        return Ok();
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignInAsync([FromBody] LoginUserInputModel model)
    {

        if (HttpContext.Request.Cookies.SingleOrDefault(x => x.Key == "bsacc").Value is not null || HttpContext.Request.Cookies.SingleOrDefault(x => x.Key == "bsrfh").Value is not null)
            return BadRequest("Você já está autenticado.");

        var cmd = new LoginUserCommand(model.UsernameOrEmail, model.Password);
        var profileId = await Sender.Send(cmd);

        if (profileId.Equals(Guid.Empty))
            return BadRequest("Usuário, email ou senha inválidos.");
        
        // Tokens, claims and cookies config
        Dictionary<string, string> claims = new()
        {
            { "ProfileId", profileId.ToString() }
        };

        HttpContext.Response.Cookies.Append("bsacc", _auth.GenerateToken(claims));
        HttpContext.Response.Cookies.Append("bsrfh", _auth.GenerateToken(claims, true), new CookieOptions() 
        {
            HttpOnly = true
        });

        return Ok();
    }
}