using BubbleSpaceApi.Api.Auth;
using BubbleSpaceApi.Api.Controllers;
using BubbleSpaceApi.Application.Commands.AskQuestionCommand;
using BubbleSpaceApi.Application.Commands.DeleteQuestionCommand;
using BubbleSpaceApi.Core.Exceptions;
using BubbleSpaceApi.Application.Models.InputModels.AskQuestionModel;
using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Application.Queries.GetQuestionQuery;
using BubbleSpaceApi.Application.Queries.GetQuestionsQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BubbleSpaceApi.Tests.ApiTests;

public class QuestionsControllerTests
{
    private readonly Mock<ISender> _senderStub;
    private readonly Mock<IAuth> _authStub;

    private readonly QuestionsController _sut;

    public QuestionsControllerTests()
    {
        _senderStub = new();
        _authStub = new();

        _sut = new(_senderStub.Object, _authStub.Object);
    }

    [Fact]
    public async Task AskAsync_ShouldReturnCreatedAt_IfExecuted()
    {
        // Arrange
        long id = 4;

        _authStub.Setup(x => x.GetProfileIdFromToken(_sut.HttpContext)).Returns(Guid.NewGuid());
        _senderStub.Setup(x => x.Send(It.IsAny<AskQuestionCommand>(), default)).ReturnsAsync(id);

        _senderStub.Setup(x => x.Send(It.IsAny<GetQuestionQuery>(), default)).ReturnsAsync(new QuestionViewModel());

        // Act
        var result = await _sut.AskAsync(new AskQuestionInputModel() { Title = "Who is here?", Description = "" });
        var okResult = result as CreatedAtActionResult;

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
        
        Assert.NotNull(okResult?.Value);
        Assert.IsType<QuestionViewModel>(okResult?.Value);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOkWithQuestions_WhenExecuted()
    {
        // Arrange
        _senderStub.Setup(x => x.Send(It.IsAny<GetQuestionsQuery>(), default)).ReturnsAsync(new List<QuestionViewModel>());

        // Act
        var result = await _sut.GetAllAsync();
        var okResult = result as OkObjectResult;

        // Assert
        Assert.IsType<OkObjectResult>(okResult);
        Assert.IsAssignableFrom<ICollection<QuestionViewModel>>(okResult?.Value);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnOkWithQuestion_WhenQuestionFound()
    {
        // Arrange
        var id = 20;
        _senderStub.Setup(x => x.Send(It.IsAny<GetQuestionQuery>(), default)).ReturnsAsync(new QuestionViewModel() { Id = id });

        // Act
        var result = (await _sut.GetAsync(id)) as OkObjectResult;
    
        // Assert
        Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(result!.Value);
        Assert.IsType<QuestionViewModel>(result.Value);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNotFound_WhenQuestionNotFound()
    {
        // Arrange
        var id = 20;
        _senderStub.Setup(x => x.Send(It.IsAny<GetQuestionQuery>(), default)).ThrowsAsync(new EntityNotFoundException("Question not found."));

        // Act
        var result = await _sut.GetAsync(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent_WhenQuestionFound()
    {
        // Arrange
        var id = 20;
        _authStub.Setup(x => x.GetProfileIdFromToken(_sut.HttpContext)).Returns(Guid.NewGuid());

        // Act
        var result = await _sut.DeleteAsync(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNotFound_WhenQuestionNotFound()
    {
        // Arrange
        var id = 20;

        _authStub.Setup(x => x.GetProfileIdFromToken(_sut.HttpContext)).Returns(Guid.NewGuid());
        _senderStub.Setup(x => x.Send(It.IsAny<DeleteQuestionCommand>(), default)).ThrowsAsync(new EntityNotFoundException("Not found question."));

        // Act
        var result = await _sut.DeleteAsync(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnForbidden_WhenUserDoesNotOwnTheQuestion()
    {
        // Arrange
        var id = 20;

        _authStub.Setup(x => x.GetProfileIdFromToken(_sut.HttpContext)).Returns(Guid.NewGuid());
        _senderStub.Setup(x => x.Send(It.IsAny<DeleteQuestionCommand>(), default)).ThrowsAsync(new ForbiddenException("User does not own the question."));

        // Act
        var result = await _sut.DeleteAsync(id);

        // Assert
        Assert.IsType<ForbidResult>(result);
    }
}