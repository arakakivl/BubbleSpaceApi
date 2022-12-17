using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Application.Queries.GetQuestionsQuery;
using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using Moq;
using Xunit;

namespace BubbleSpaceApi.ApplicationTests.Queries;

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
        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntitiesAsync(null, "Profile,Answers")).ReturnsAsync(new List<Question>().AsQueryable());
        _unitOfWorkStub.Setup(x => x.ProfileRepository.GetEntityAsync(It.IsAny<Guid>())).ReturnsAsync(new Profile());
        
        var query = new GetQuestionsQuery();

        // Act
        var result = await _sut.Handle(query, default);

        // Assert
        Assert.IsAssignableFrom<ICollection<QuestionViewModel>>(result);
    }
}