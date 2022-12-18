using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Application.Queries.GetProfileQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BubbleSpaceApi.Core.Communication.Handlers;

namespace BubbleSpaceApi.Api.Controllers;

public class ProfileController : ApiControllerBase
{
    public ProfileController(ISender sender, IDomainNotificationHandler domainNotificationHandler) : base(sender, domainNotificationHandler)
    {

    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetByUsernameAsync([FromRoute] string? username)
    {
        var cmd = new GetProfileQuery(username ??= "");
        
        try
        {
            var profile = await Sender.Send(cmd);
            return Ok(profile);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}