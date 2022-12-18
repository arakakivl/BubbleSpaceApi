using BubbleSpaceApi.Api.Auth;
using BubbleSpaceApi.Api.Controllers;
using BubbleSpaceApi.Application.Commands.AnswerQuestionCommand;
using BubbleSpaceApi.Application.Commands.DeleteAnswerCommand;
using BubbleSpaceApi.Application.Commands.DeleteQuestionCommand;
using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Application.Models.InputModels.AnswerQuestionModel;
using BubbleSpaceApi.Application.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using AutoFixture;
using BubbleSpaceApi.Core.Communication.Handlers;

namespace BubbleSpaceApi.ApiTests;

public class AnswersControllerTests
{
    private readonly Fixture _fixture;

    private readonly Mock<ISender> _senderStub;
    private readonly Mock<IAuth> _authStub;
    private readonly Mock<IDomainNotificationHandler> _domainNotificationHandlerStub;

    private readonly AnswersController _sut;

    public AnswersControllerTests()
    {
        _fixture = new();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(x => _fixture.Behaviors.Remove(x));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _senderStub = new();
        _authStub = new();
        _domainNotificationHandlerStub = new();

        _sut = new(_senderStub.Object, _authStub.Object, _domainNotificationHandlerStub.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        _sut.HttpContext.Request.Headers.Authorization = "Bearer someSortOfJWTToken";
    }

    [Fact]
    public async Task AnswerAsync_ShouldReturnOk_WhenUserDoesNotAnsweredYet()
    {
        // Arrange
        var model = _fixture.Build<AnswerQuestionInputModel>().With(x => x.Text, "answer").Create();
        _authStub.Setup(x => x.GetProfileIdFromToken(It.IsAny<string>())).Returns(Guid.NewGuid());

        // Act
        var result = (await _sut.AnswerAsync(1, model));

        // Assert
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<ResultViewModel>((result as ObjectResult)!.Value);
    }

    [Fact]
    public async Task AnswerAsync_ShouldReturnBadRequest_WhenUserAlreadyAnsweredQuestion()
    {
        // Arrange
        var model = _fixture.Build<AnswerQuestionInputModel>().With(x => x.Text, "answer").Create();

        _authStub.Setup(x => x.GetProfileIdFromToken(It.IsAny<string>())).Returns(Guid.NewGuid());
        _senderStub.Setup(x => x.Send(It.IsAny<AnswerQuestionCommand>(), default)).ThrowsAsync(new AlreadyAnsweredQuestionException("Already answered question."));

        // Act
        var result = await _sut.AnswerAsync(1, model);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task AnswerAsync_ShouldReturnNotFound_WhenQuestionNotFound()
    {
        // Arrange
        var model = _fixture.Build<AnswerQuestionInputModel>().With(x => x.Text, "answer").Create();

        _authStub.Setup(x => x.GetProfileIdFromToken(It.IsAny<string>())).Returns(Guid.NewGuid());
        _senderStub.Setup(x => x.Send(It.IsAny<AnswerQuestionCommand>(), default)).ThrowsAsync(new EntityNotFoundException("Question not found."));

        // Act
        var result = await _sut.AnswerAsync(-1, model);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent_WhenQuestionFound()
    {
        // Arrange
        _authStub.Setup(x => x.GetProfileIdFromToken(It.IsAny<string>())).Returns(Guid.NewGuid());

        // Act
        var result = await _sut.DeleteAsync(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNotFound_WhenQuestionOrAnswerNotFound()
    {
        // Arrange
        _authStub.Setup(x => x.GetProfileIdFromToken(It.IsAny<string>())).Returns(Guid.NewGuid());
        _senderStub.Setup(x => x.Send(It.IsAny<DeleteAnswerCommand>(), default)).ThrowsAsync(new EntityNotFoundException("Question or answer not found."));
        
        // Act
        var result = await _sut.DeleteAsync(-1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}