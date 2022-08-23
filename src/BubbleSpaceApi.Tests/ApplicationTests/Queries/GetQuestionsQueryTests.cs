using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Application.Queries.GetQuestionsQuery;
using BubbleSpaceApi.Core.Entities;
using BubbleSpaceApi.Core.Interfaces;
using Moq;
using Xunit;

namespace BubbleSpaceApi.Tests.ApplicationTests.QueriesTests;

public class GetQuestionsQueryTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly GetQuestionsQueryHandler _sut;

    public GetQuestionsQueryTests()
    {
        _unitOfWorkStub = new();
        _sut = new(_unitOfWorkStub.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnQuestions_WhenExecuted()
    {
        // Arrange
        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntitiesAsync(null, "Profile,Answers")).ReturnsAsync(new List<Question>());
        var query = new GetQuestionsQuery();

        // Act
        var result = await _sut.Handle(query, default);

        // Assert
        Assert.IsAssignableFrom<ICollection<QuestionViewModel>>(result);
    }
}