using BubbleSpaceApi.Api.Auth;
using BubbleSpaceApi.Api.Controllers;
using BubbleSpaceApi.Application.Commands.AnswerQuestionCommand;
using BubbleSpaceApi.Application.Commands.DeleteAnswerCommand;
using BubbleSpaceApi.Application.Commands.DeleteQuestionCommand;
using BubbleSpaceApi.Core.Exceptions;
using BubbleSpaceApi.Application.Models.InputModels.AnswerQuestionModel;
using BubbleSpaceApi.Application.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BubbleSpaceApi.ApiTests;

public class AnswersControllerTests
{
    private readonly Mock<ISender> _senderStub;
    private readonly Mock<IAuth> _authStub;

    private readonly AnswersController _sut;

    public AnswersControllerTests()
    {
        _senderStub = new();
        _authStub = new();

        _sut = new(_senderStub.Object, _authStub.Object);
    }

    [Fact]
    public async Task AnswerAsync_ShouldReturnOk_WhenUserDoesNotAnsweredYet()
    {
        // Arrange
        var qId = 4;
        var model = new AnswerQuestionInputModel() { Text = "my answer goes here!" };
        
        _authStub.Setup(x => x.GetProfileIdFromToken(_sut.HttpContext)).Returns(Guid.NewGuid());

        // Act
        var result = (await _sut.AnswerAsync(qId, model));

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task AnswerAsync_ShouldReturnBadRequest_WhenUserAlreadyAnsweredQuestion()
    {
        // Arrange
        var qId = 4;
        var model = new AnswerQuestionInputModel() { Text = "my answer goes here!" };

        _authStub.Setup(x => x.GetProfileIdFromToken(_sut.HttpContext)).Returns(Guid.NewGuid());
        _senderStub.Setup(x => x.Send(It.IsAny<AnswerQuestionCommand>(), default)).ThrowsAsync(new AlreadyAnsweredQuestionException("Already answered question."));

        // Act
        var result = await _sut.AnswerAsync(qId, model);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task AnswerAsync_ShouldReturnNotFound_WhenQuestionNotFound()
    {
        // Arrange
        var qId = 4;
        var model = new AnswerQuestionInputModel() { Text = "my answer goes here!" };

        _authStub.Setup(x => x.GetProfileIdFromToken(_sut.HttpContext)).Returns(Guid.NewGuid());
        _senderStub.Setup(x => x.Send(It.IsAny<AnswerQuestionCommand>(), default)).ThrowsAsync(new EntityNotFoundException("Question not found."));

        // Act
        var result = await _sut.AnswerAsync(qId, model);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent_WhenQuestionFound()
    {
        // Arrange
        var qId = 4;

        _authStub.Setup(x => x.GetProfileIdFromToken(_sut.HttpContext)).Returns(Guid.NewGuid());

        // Act
        var result = await _sut.DeleteAsync(qId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNotFound_WhenQuestionOrAnswerNotFound()
    {
        // Arrange
        var qId = 4;

        _authStub.Setup(x => x.GetProfileIdFromToken(_sut.HttpContext)).Returns(Guid.NewGuid());
        _senderStub.Setup(x => x.Send(It.IsAny<DeleteAnswerCommand>(), default)).ThrowsAsync(new EntityNotFoundException("Question or answer not found."));
        
        // Act
        var result = await _sut.DeleteAsync(qId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}