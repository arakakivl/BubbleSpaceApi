using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Application.Queries.GetQuestionQuery;
using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using Moq;
using Xunit;

namespace BubbleSpaceApi.ApplicationTests.Queries;

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

        Profile p = new Profile();
        List<Answer> answers = new(); 

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntitiesAsync(q => q.Id == query.Id, "Profile,Answers")).ReturnsAsync(new List<Question>() { new Question() { Id = id, Profile = p, Answers = answers  } }.AsQueryable());
        _unitOfWorkStub.Setup(x => x.ProfileRepository.GetEntityAsync(It.IsAny<Guid>())).ReturnsAsync(new Profile());

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

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntitiesAsync(q => q.Id == query.Id, "Profile,Answers")).ReturnsAsync(new List<Question>() {   }.AsQueryable());
        _unitOfWorkStub.Setup(x => x.ProfileRepository.GetEntityAsync(It.IsAny<Guid>())).ReturnsAsync(new Profile());

        // Act
        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.Handle(query, default));
    }
}