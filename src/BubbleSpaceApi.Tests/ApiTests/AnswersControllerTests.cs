using BubbleSpaceApi.Api.Controllers;
using BubbleSpaceApi.Application.Commands.AnswerQuestionCommand;
using BubbleSpaceApi.Application.Exceptions;
using BubbleSpaceApi.Application.Models.InputModels.AnswerQuestionModel;
using BubbleSpaceApi.Application.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BubbleSpaceApi.Tests.ApiTests;

public class AnswersControllerTests
{
    private readonly Mock<ISender> _senderStub;
    private readonly AnswersController _sut;

    public AnswersControllerTests()
    {
        _senderStub = new();
        _sut = new(_senderStub.Object);
    }

    [Fact]
    public async Task AnswerAsync_ShouldReturnCreatedAt_WhenUserDoesNotAnsweredYet()
    {
        // Arrange
        var qId = 4;
        var model = new AnswerQuestionInputModel() { Text = "my answer goes here!" };
        
        // Act
        var result = (await _sut.AnswerAsync(qId, model)) as CreatedAtRouteResult;

        // Assert
        Assert.IsType<CreatedAtRouteResult>(result);

        Assert.NotNull(result!.Value);
        Assert.IsType<AnswerViewModel>(result.Value);
    }

    [Fact]
    public async Task AnswerAsync_ShouldReturnBadRequest_WhenUserAlreadyAnsweredQuestion()
    {
        // Arrange
        var qId = 4;
        var model = new AnswerQuestionInputModel() { Text = "my answer goes here!" };

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
        _senderStub.Setup(x => x.Send(It.IsAny<AnswerQuestionCommand>(), default)).ThrowsAsync(new AlreadyAnsweredQuestionException("Question or answer not found."));
        
        // Act
        var result = await _sut.DeleteAsync(qId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}