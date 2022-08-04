using Microsoft.AspNetCore.Mvc;

namespace BubbleSpaceApi.Api.Controllers;

public class QuestionsController : BaseSenderController
{
    [HttpPost("ask")]
    public async Task<IActionResult> AskAsync()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<IActionResult> GetallAsync()
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] long id)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] long id)
    {
        throw new NotImplementedException();
    }
}