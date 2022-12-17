using BubbleSpaceApi.Application.Commands.DeleteQuestionCommand;
using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using Moq;
using Xunit;
using AutoFixture;

namespace BubbleSpaceApi.ApplicationTests.Commands;

public class DeleteQuestionCommandTests
{
    private readonly Fixture _fixture;

    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly DeleteQuestionCommandHandler _sut;

    public DeleteQuestionCommandTests()
    {
        _fixture = new();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(x => _fixture.Behaviors.Remove(x));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkStub = new();
        _sut = new(_unitOfWorkStub.Object);
    }

    [Fact]
    public async Task Handle_ShouldDelete_WhenFoundQuestion()
    {
        // Arrange
        var cmd = _fixture.Create<DeleteQuestionCommand>();
        var question = _fixture.Build<Question>().With(x => x.Id, cmd.QuestionId).With(x => x.ProfileId, cmd.ProfileId).Create();

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntityAsync(cmd.QuestionId)).ReturnsAsync(question);

        _unitOfWorkStub.Setup(x => x.QuestionRepository.DeleteAsync(question.Id)).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Act
        await _sut.Handle(cmd, default);

        // Assert
        _unitOfWorkStub.Verify(x => x.QuestionRepository.DeleteAsync(question.Id), Times.Once);
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowEntityNotFoundException_WhenNotFoundQuestion()
    {
        // Arrange
        var cmd = _fixture.Create<DeleteQuestionCommand>();
        var question = _fixture.Build<Question>().With(x => x.Id, cmd.QuestionId).With(x => x.ProfileId, cmd.ProfileId).Create();

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntityAsync(cmd.QuestionId)).ReturnsAsync((Question?)null);

        _unitOfWorkStub.Setup(x => x.QuestionRepository.DeleteAsync(question.Id)).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>( async () => await _sut.Handle(cmd, default));

        _unitOfWorkStub.Verify(x => x.QuestionRepository.DeleteAsync(question.Id), Times.Never);
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowForbiddenException_WhenQuestionIsNotFromUser()
    {
        // Arrange
        var cmd = _fixture.Create<DeleteQuestionCommand>();
        var question = _fixture.Build<Question>().With(x => x.Id, cmd.QuestionId).Create();

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntityAsync(cmd.QuestionId)).ReturnsAsync(question);

        _unitOfWorkStub.Setup(x => x.QuestionRepository.DeleteAsync(question.Id)).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Assert
        await Assert.ThrowsAsync<ForbiddenException>(async () => await _sut.Handle(cmd, default));

        _unitOfWorkStub.Verify(x => x.QuestionRepository.DeleteAsync(question.Id), Times.Never);
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}