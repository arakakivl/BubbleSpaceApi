using MediatR;
using Microsoft.AspNetCore.Mvc;
using BubbleSpaceApi.Application.Models.InputModels.RegisterUserModel;
using BubbleSpaceApi.Application.Commands.RegisterUserCommand;
using BubbleSpaceApi.Api.Auth;
using BubbleSpaceApi.Application.Commands.LoginUserCommand;
using Microsoft.AspNetCore.Authorization;
using BubbleSpaceApi.Application.Models.InputModels.LoginUserModel;
using BubbleSpaceApi.Core.Communication.Handlers;

namespace BubbleSpaceApi.Api.Controllers;

[AllowAnonymous]
public class AccountController : ApiControllerBase
{
    private readonly IAuth _auth;
    public AccountController(ISender sender, IAuth auth, IDomainNotificationHandler domainNotificationHandler) : base(sender, domainNotificationHandler)
    {
        _auth = auth;
    }
    
    [HttpPost("signup")]
    public async Task<IActionResult> SignUpAsync([FromBody] RegisterUserInputModel model)
    {
        if (_auth.IsAuthenticated(GetAuthorizationBearerToken()) || _auth.IsAuthenticated(GetRefreshCookieToken()))
            return BadRequest("Você já está autenticado.");
        else if (!model.IsValid())
            return model.ReturnUnprocessableEntity();

        var cmd = new RegisterUserCommand(model.Username, model.Email, model.Password);
        await Sender.Send(cmd);

        return Ok();
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignInAsync([FromBody] LoginUserInputModel model)
    {
        if (_auth.IsAuthenticated(GetAuthorizationBearerToken()) || _auth.IsAuthenticated(GetRefreshCookieToken()))
            return BadRequest("Você já está autenticado.");
        else if (!model.IsValid())
            return model.ReturnUnprocessableEntity();

        var cmd = new LoginUserCommand(model.UsernameOrEmail, model.Password);
        var profileId = await Sender.Send(cmd);

        if (profileId.Equals(Guid.Empty))
            return BadRequest("Usuário, email ou senha inválidos.");
        
        // Tokens, claims and cookies config
        Dictionary<string, string> claims = new() 
        { { "ProfileId", profileId.ToString() } };

        HttpContext.Response.Cookies.Append("bsrfh", _auth.GenerateToken(claims, true), new CookieOptions() 
        { HttpOnly = true });

        return Ok(new { bsacc = _auth.GenerateToken(claims) });
    }

    [HttpPost("bsrfht")]
    public async Task<IActionResult> RefreshAsync()
    {
        if (_auth.IsAuthenticated(GetAuthorizationBearerToken()) || _auth.IsAuthenticated(GetRefreshCookieToken()))
            return BadRequest("Você já está autenticado.");
    
        // Tokens, claims and cookies config
        var profileId = _auth.GetProfileIdFromToken(GetRefreshCookieToken());

        Dictionary<string, string> claims = new() 
        { { "ProfileId", profileId.ToString() } };

        HttpContext.Response.Cookies.Append("bsrfh", _auth.GenerateToken(claims, true), new CookieOptions() 
        { HttpOnly = true });

        return await Task.FromResult(Ok(new { bsacc = _auth.GenerateToken(claims) }));
    }
}