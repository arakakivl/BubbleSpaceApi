using BubbleSpaceApi.Application.Commands.AnswerQuestionCommand;
using BubbleSpaceApi.Application.Commands.DeleteAnswerCommand;
using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using Moq;
using Xunit;
using AutoFixture;

namespace BubbleSpaceApi.ApplicationTests.Commands;

public class DeleteAnswerCommandTests
{
    private readonly Fixture _fixture;

    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly DeleteAnswerCommandHandler _sut;

    public DeleteAnswerCommandTests()
    {
        _fixture = new();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(x => _fixture.Behaviors.Remove(x));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkStub = new();
        _sut = new(_unitOfWorkStub.Object);
    }

    [Fact]
    public async Task Handle_ShouldDelete_WhenExecuted()
    {
        // Arrange
        var cmd = _fixture.Create<DeleteAnswerCommand>();

        var question = _fixture.Build<Question>().With(x => x.Id, cmd.QuestionId).Create();
        question.Answers = new List<Answer>() { _fixture.Build<Answer>().With(x => x.ProfileId, cmd.ProfileId).Create() };

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntitiesAsync(q => q.Id == cmd.QuestionId, "Answers")).ReturnsAsync(new List<Question>() { question }.AsQueryable());

        _unitOfWorkStub.Setup(x => x.AnswerRepository.DeleteAsync(It.IsAny<Guid>())).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Act
        await _sut.Handle(cmd, default);

        // Assert
        _unitOfWorkStub.Verify(x => x.AnswerRepository.DeleteAsync(It.IsAny<Guid>()), Times.Once());
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Once());
    }

    [Fact]
    public async Task Handle_ShouldThrowEntityNotFoundException_WhenInexistentQuestion()
    {
        // Arrange
        var cmd = _fixture.Create<DeleteAnswerCommand>();

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntitiesAsync(q => q.Id == cmd.QuestionId, "Answers")).ReturnsAsync(new List<Question>() {  }.AsQueryable());

        _unitOfWorkStub.Setup(x => x.AnswerRepository.DeleteAsync(It.IsAny<Guid>())).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.Handle(cmd, default));

        _unitOfWorkStub.Verify(x => x.AnswerRepository.DeleteAsync(It.IsAny<Guid>()), Times.Never());
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Never());
    }

    [Fact]
    public async Task Handle_ShouldThrowEntityNotFoundException_WhenUserNotAnswered()
    {
        // Arrange
        var cmd = _fixture.Create<DeleteAnswerCommand>();
        var question = _fixture.Build<Question>().With(x => x.Id, cmd.QuestionId).With(x => x.Answers, new List<Answer>()).Create();

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntitiesAsync(q => q.Id == cmd.QuestionId, "Answers")).ReturnsAsync(new List<Question>() { question }.AsQueryable());

        _unitOfWorkStub.Setup(x => x.AnswerRepository.DeleteAsync(It.IsAny<Guid>())).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.Handle(cmd, default));

        _unitOfWorkStub.Verify(x => x.AnswerRepository.DeleteAsync(It.IsAny<Guid>()), Times.Never());
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Never());
    }
}