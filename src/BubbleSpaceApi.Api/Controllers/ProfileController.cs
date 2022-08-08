using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BubbleSpaceApi.Api.Controllers;

public class ProfileController
{
    private readonly ISender _sender;
    public ProfileController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetByUsernameAsync([FromQuery] string? username)
    {
        throw new NotImplementedException();
    }
}