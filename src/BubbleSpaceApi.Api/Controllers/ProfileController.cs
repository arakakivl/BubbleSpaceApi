using BubbleSpaceApi.Application.Exceptions;
using BubbleSpaceApi.Application.Queries.GetProfileQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BubbleSpaceApi.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("/profile")]
public class ProfileController : ControllerBase
{
    private readonly ISender _sender;
    public ProfileController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetByUsernameAsync([FromQuery] string? username)
    {
        var cmd = new GetProfileQuery(username ??= "");
        
        try
        {
            var profile = await _sender.Send(cmd);
            return Ok(profile);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}