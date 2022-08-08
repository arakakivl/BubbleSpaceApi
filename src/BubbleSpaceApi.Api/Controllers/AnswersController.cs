using BubbleSpaceApi.Application.Models.InputModels.AnswerQuestionModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BubbleSpaceApi.Api.Controllers;

public class AnswersController
{
    private readonly ISender _sender;
    public AnswersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("{questionId}")]
    public async Task<IActionResult> AnswerAsync([FromRoute] long questionId, [FromBody] AnswerQuestionInputModel model)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{questionId}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] long questionId)
    {
        throw new NotImplementedException();
    }
}