using BubbleSpaceApi.Api.Auth;
using BubbleSpaceApi.Application.Commands.AnswerQuestionCommand;
using BubbleSpaceApi.Application.Commands.DeleteAnswerCommand;
using BubbleSpaceApi.Application.Exceptions;
using BubbleSpaceApi.Application.Models.InputModels.AnswerQuestionModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BubbleSpaceApi.Api.Controllers;

[Authorize]
[ApiController]
[Route("/answers")]
public class AnswersController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IAuth _auth;
   
    public AnswersController(ISender sender, IAuth auth)
    {
        _sender = sender;
        _auth = auth;
    }

    [HttpPost("{questionId}")]
    public async Task<IActionResult> AnswerAsync([FromRoute] long questionId, [FromBody] AnswerQuestionInputModel model)
    {
        try
        {
            var profileId = _auth.GetProfileIdFromToken(HttpContext);
            var cmd = new AnswerQuestionCommand(questionId, profileId, model.Text);

            await _sender.Send(cmd);

            return Ok();
        }
        catch (Exception e)
        {
            if (e is EntityNotFoundException)
                return NotFound(e.Message);
            else if (e is AlreadyAnsweredQuestionException)
                return BadRequest(e.Message);
            else
                return BadRequest();
        }
    }

    [HttpDelete("{questionId}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] long questionId)
    {
        try
        {
            var profileId = _auth.GetProfileIdFromToken(HttpContext);
            var cmd = new DeleteAnswerCommand(questionId, profileId);

            await _sender.Send(cmd);

            return NoContent();
        }
        catch (Exception e)
        {
            if (e is EntityNotFoundException)
                return NotFound(e.Message);
            else
                return BadRequest();
        }
    }
}