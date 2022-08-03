using BubbleSpaceApi.Application.Exceptions;
using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Application.Queries.GetQuestionQuery;
using BubbleSpaceApi.Core.Entities;
using BubbleSpaceApi.Core.Interfaces;
using Moq;
using Xunit;

namespace BubbleSpaceApi.Tests.ApplicationTests.QueriesTests;

public class GetQuestionQueryTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly GetQuestionQueryHandler _sut;

    public GetQuestionQueryTests()
    {
        _unitOfWorkStub = new();
        _sut = new(_unitOfWorkStub.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnQuestion_WhenExistentQuestion()
    {
        // Arrange
        long id = 10;
        var query = new GetQuestionQuery(id);

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntityAsync(id)).ReturnsAsync(new Question());

        // Act
        var result = await _sut.Handle(query, default);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<QuestionViewModel>(result);
    }

    [Fact]
    public async Task Handle_ShouldThrowEntityNotFoundException_WhenQuestionNotFound()
    {
        // Arrange
        long id = 10;
        var query = new GetQuestionQuery(id);

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntityAsync(id)).ReturnsAsync((Question?)null);

        // Act
        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.Handle(query, default));
    }
}