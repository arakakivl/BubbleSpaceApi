using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Application.Queries.GetQuestionQuery;
using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using Moq;
using Xunit;
using AutoFixture;
using BubbleSpaceApi.Core.Communication.Mediator;

namespace BubbleSpaceApi.ApplicationTests.Queries;

public class GetQuestionQueryTests
{
    private readonly Fixture _fixture;

    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly Mock<IMediatorHandler> _mediatorHandler;
    
    private readonly GetQuestionQueryHandler _sut;

    public GetQuestionQueryTests()
    {
        _fixture = new();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(x => _fixture.Behaviors.Remove(x));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkStub = new();
        _mediatorHandler = new();

        _sut = new(_unitOfWorkStub.Object, _mediatorHandler.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnQuestion_WhenExistentQuestion()
    {
        // Arrange
        var query = _fixture.Create<GetQuestionQuery>();
        var question = _fixture.Build<Question>().With(x => x.Id, query.QuestionId).Create();

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntitiesAsync(q => q.Id == query.QuestionId, "Profile,Answers")).ReturnsAsync(new List<Question>() { question }.AsQueryable());
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
        var query = _fixture.Create<GetQuestionQuery>();

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntitiesAsync(q => q.Id == query.QuestionId, "Profile,Answers")).ReturnsAsync(new List<Question>().AsQueryable());
        _unitOfWorkStub.Setup(x => x.ProfileRepository.GetEntityAsync(It.IsAny<Guid>())).ReturnsAsync(new Profile());

        // Act
        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.Handle(query, default));
    }
}