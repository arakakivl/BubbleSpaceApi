using AutoFixture;
using BubbleSpaceApi.Application.Commands.AskQuestionCommand;
using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using Moq;
using Xunit;

namespace BubbleSpaceApi.ApplicationTests.Commands;

public class AskQuestionCommandTests
{
    private readonly Fixture _fixture;

    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly AskQuestionCommandHandler _sut;

    public AskQuestionCommandTests()
    {
        _fixture = new();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(x => _fixture.Behaviors.Remove(x));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkStub = new();
        _sut = new(_unitOfWorkStub.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddToDatabase_WhenExecuted()
    {
        // Arrange
        var question = _fixture.Create<Question>();
        var cmd = _fixture.Build<AskQuestionCommand>().With(x => x.ProfileId, question.ProfileId).Create();

        _unitOfWorkStub.Setup(x => x.QuestionRepository.AddAsync(question)).ReturnsAsync(question.Id);

        _unitOfWorkStub.Setup(x => x.QuestionRepository.AddAsync(It.IsAny<Question>())).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();
    
        // Act
        var r = await _sut.Handle(cmd, default);

        // Assert
        _unitOfWorkStub.Verify(x => x.QuestionRepository.AddAsync(It.IsAny<Question>()), Times.Once);
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}