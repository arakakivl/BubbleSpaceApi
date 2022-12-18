using BubbleSpaceApi.Api.Auth;
using BubbleSpaceApi.Application.Commands.AnswerQuestionCommand;
using BubbleSpaceApi.Application.Commands.DeleteAnswerCommand;
using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Application.Models.InputModels.AnswerQuestionModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BubbleSpaceApi.Core.Communication.Handlers;

namespace BubbleSpaceApi.Api.Controllers;

[Authorize]
public class AnswersController : ApiControllerBase
{
    private readonly IAuth _auth;

    public AnswersController(ISender sender, IAuth auth, IDomainNotificationHandler domainNotificationHandler) : base(sender, domainNotificationHandler)
    {
        _auth = auth;
    }

    [HttpPost("{questionId}")]
    public async Task<IActionResult> AnswerAsync([FromRoute] long questionId, [FromBody] AnswerQuestionInputModel model)
    {
        if (!model.IsValid())
            return model.ReturnUnprocessableEntity();

        try
        {
            var profileId = _auth.GetProfileIdFromToken(GetAuthorizationBearerToken());
            var cmd = new AnswerQuestionCommand(questionId, profileId, model.Text);

            await Sender.Send(cmd);

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
            var profileId = _auth.GetProfileIdFromToken(GetAuthorizationBearerToken());
            var cmd = new DeleteAnswerCommand(questionId, profileId);

            await Sender.Send(cmd);

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