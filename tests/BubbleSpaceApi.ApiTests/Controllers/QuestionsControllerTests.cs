using BubbleSpaceApi.Api.Auth;
using BubbleSpaceApi.Api.Controllers;
using BubbleSpaceApi.Application.Commands.AskQuestionCommand;
using BubbleSpaceApi.Application.Commands.DeleteQuestionCommand;
using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Application.Models.InputModels.AskQuestionModel;
using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Application.Queries.GetQuestionQuery;
using BubbleSpaceApi.Application.Queries.GetQuestionsQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using AutoFixture;

namespace BubbleSpaceApi.ApiTests;

public class QuestionsControllerTests
{
    private readonly Fixture _fixture;

    private readonly Mock<ISender> _senderStub;
    private readonly Mock<IAuth> _authStub;

    private readonly QuestionsController _sut;

    public QuestionsControllerTests()
    {
        _fixture = new();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(x => _fixture.Behaviors.Remove(x));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _senderStub = new();
        _authStub = new();

        _sut = new(_senderStub.Object, _authStub.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        _sut.ControllerContext.HttpContext.Request.Headers.Authorization = "Bearer someSortOfJWTToken";
    }

    [Fact]
    public async Task AskAsync_ShouldReturnCreatedAt_IfExecuted()
    {
        // Arrange
        var model = _fixture.Build<AskQuestionInputModel>().With(x => x.Title, "Question").Create();
        var viewmodel = _fixture.Create<QuestionViewModel>();

        _authStub.Setup(x => x.GetProfileIdFromToken(It.IsAny<string>())).Returns(Guid.NewGuid());
        _senderStub.Setup(x => x.Send(It.IsAny<AskQuestionCommand>(), default)).ReturnsAsync(0);

        _senderStub.Setup(x => x.Send(It.IsAny<GetQuestionQuery>(), default)).ReturnsAsync(viewmodel);

        // Act
        var result = await _sut.AskAsync(model);
        var okResult = result as CreatedAtActionResult;

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
        
        Assert.NotNull(okResult?.Value);
        Assert.IsType<ResultViewModel>(okResult?.Value);
    }

    [Fact]
    public async Task AskAsync_ShouldReturnUnprocessableEntity_WhenInvalidModelProvided()
    {
        // Arrange
        var model = _fixture.Build<AskQuestionInputModel>().With(x => x.Title, "").Create();
        _authStub.Setup(x => x.GetProfileIdFromToken(It.IsAny<string>())).Returns(Guid.NewGuid());

        // Act
        var result = await _sut.AskAsync(model);

        // Assert
        Assert.IsType<UnprocessableEntityObjectResult>(result);
        Assert.IsType<ResultViewModel>((result as ObjectResult)!.Value);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOkWithQuestions_WhenExecuted()
    {
        // Arrange
        var viewmodels = _fixture.CreateMany<QuestionViewModel>().ToList();
        _senderStub.Setup(x => x.Send(It.IsAny<GetQuestionsQuery>(), default)).ReturnsAsync(viewmodels);

        // Act
        var result = await _sut.GetAllAsync();
        var okResult = result as OkObjectResult;

        // Assert
        Assert.IsType<OkObjectResult>(okResult);
        Assert.IsType<ResultViewModel>(okResult.Value);
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
        Assert.IsType<ResultViewModel>(result.Value);

        Assert.NotNull((result.Value as ResultViewModel)!.Data);
        Assert.IsType<QuestionViewModel>((result.Value as ResultViewModel)!.Data);
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
        Assert.IsType<ResultViewModel>((result as ObjectResult)!.Value);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent_WhenQuestionFound()
    {
        // Arrange
        var id = 20;
        _authStub.Setup(x => x.GetProfileIdFromToken(It.IsAny<string>())).Returns(Guid.NewGuid());

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

        _authStub.Setup(x => x.GetProfileIdFromToken(It.IsAny<string>())).Returns(Guid.NewGuid());
        _senderStub.Setup(x => x.Send(It.IsAny<DeleteQuestionCommand>(), default)).ThrowsAsync(new EntityNotFoundException("Not found question."));

        // Act
        var result = await _sut.DeleteAsync(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.IsType<ResultViewModel>((result as ObjectResult)!.Value);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnForbidden_WhenUserDoesNotOwnTheQuestion()
    {
        // Arrange
        var id = 20;

        _authStub.Setup(x => x.GetProfileIdFromToken(It.IsAny<string>())).Returns(Guid.NewGuid());
        _senderStub.Setup(x => x.Send(It.IsAny<DeleteQuestionCommand>(), default)).ThrowsAsync(new ForbiddenException("User does not own the question."));

        // Act
        var result = await _sut.DeleteAsync(id);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.IsType<ResultViewModel>((result as ObjectResult)!.Value);
    }
}