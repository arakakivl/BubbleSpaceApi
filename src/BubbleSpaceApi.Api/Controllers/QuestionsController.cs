using BubbleSpaceApi.Api.Auth;
using BubbleSpaceApi.Application.Commands.AskQuestionCommand;
using BubbleSpaceApi.Application.Commands.DeleteQuestionCommand;
using BubbleSpaceApi.Application.Exceptions;
using BubbleSpaceApi.Application.Models.InputModels.AskQuestionModel;
using BubbleSpaceApi.Application.Queries.GetQuestionQuery;
using BubbleSpaceApi.Application.Queries.GetQuestionsQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BubbleSpaceApi.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("/questions")]
public class QuestionsController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IAuth _auth;

    public QuestionsController(ISender sender, IAuth auth)
    {
        _sender = sender;
        _auth = auth;
    }
    
    [Authorize]
    [HttpPost("ask")]
    public async Task<IActionResult> AskAsync([FromBody] AskQuestionInputModel model)
    {
        var profId = _auth.GetProfileIdFromToken(HttpContext);
        var cmd = new AskQuestionCommand(profId, model.Title, model.Description);

        var r = await _sender.Send(cmd);
        var q = await _sender.Send(new GetQuestionQuery(r));

        if (q is null)
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");

        return CreatedAtAction(nameof(GetAsync), new { Id = r }, q);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var cmd = new GetQuestionsQuery();
        var r = await _sender.Send(cmd);

        return Ok(r);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] long id)
    {
        try
        {
            var cmd = new GetQuestionQuery(id);
            var r = await _sender.Send(cmd);

            return Ok(r);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] long id)
    {
        try
        {
            var profId = _auth.GetProfileIdFromToken(HttpContext);
            var cmd = new DeleteQuestionCommand(id, profId);

            await _sender.Send(cmd);

            return NoContent();
        }
        catch (Exception e)
        {
            if (e is SecurityTokenException || e is ForbiddenException)
                return Forbid("Não autorizado.");
            else if (e is EntityNotFoundException)
                return NotFound(e.Message);
            else
                return BadRequest();
        }
    }
}