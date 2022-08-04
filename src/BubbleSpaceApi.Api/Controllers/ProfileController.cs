using Microsoft.AspNetCore.Mvc;

namespace BubbleSpaceApi.Api.Controllers;

public class ProfileController : BaseSenderController
{
    [HttpGet("{username}")]
    public async Task<IActionResult> GetByUsernameAsync([FromQuery] string? username)
    {
        throw new NotImplementedException();
    }
}