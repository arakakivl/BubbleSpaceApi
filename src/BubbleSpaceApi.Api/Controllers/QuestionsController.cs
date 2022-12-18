using BubbleSpaceApi.Api.Auth;
using BubbleSpaceApi.Application.Commands.AskQuestionCommand;
using BubbleSpaceApi.Application.Commands.DeleteQuestionCommand;
using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Application.Models.InputModels.AskQuestionModel;
using BubbleSpaceApi.Application.Queries.GetQuestionQuery;
using BubbleSpaceApi.Application.Queries.GetQuestionsQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BubbleSpaceApi.Core.Communication.Handlers;

namespace BubbleSpaceApi.Api.Controllers;

[Authorize]
public class QuestionsController : ApiControllerBase
{
    private readonly IAuth _auth;
    public QuestionsController(ISender sender, IAuth auth, IDomainNotificationHandler domainNotificationHandler) : base(sender, domainNotificationHandler)
    {
        _auth = auth;
    }
    
    [HttpPost("ask")]
    public async Task<IActionResult> AskAsync([FromBody] AskQuestionInputModel model)
    {
        if (!model.IsValid())
            return model.ReturnUnprocessableEntity();

        var profId = _auth.GetProfileIdFromToken(GetAuthorizationBearerToken());
        var cmd = new AskQuestionCommand(profId, model.Title, model.Description);

        var r = await Sender.Send(cmd);
        var q = await Sender.Send(new GetQuestionQuery(r));

        return CreatedAtAction(nameof(GetAsync), new { Id = r }, q);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var cmd = new GetQuestionsQuery();
        var r = await Sender.Send(cmd);

        return Ok(r);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] long id)
    {
        try
        {
            var cmd = new GetQuestionQuery(id);
            var r = await Sender.Send(cmd);

            return Ok(r);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] long id)
    {
        try
        {
            var profId = _auth.GetProfileIdFromToken(GetAuthorizationBearerToken());
            var cmd = new DeleteQuestionCommand(id, profId);

            await Sender.Send(cmd);

            return NoContent();
        }
        catch (Exception e)
        {
            if (e is SecurityTokenException || e is ForbiddenException)
                return Unauthorized("Não autorizado.");
            else if (e is EntityNotFoundException)
                return NotFound(e.Message);
            else
                return BadRequest();
        }
    }
}