using BubbleSpaceApi.Application.Commands.DeleteQuestionCommand;
using BubbleSpaceApi.Core.Exceptions;
using BubbleSpaceApi.Core.Entities;
using BubbleSpaceApi.Core.Interfaces;
using Moq;
using Xunit;

namespace BubbleSpaceApi.Tests.ApplicationTests.CommandTests;

public class DeleteQuestionCommandTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly DeleteQuestionCommandHandler _sut;

    public DeleteQuestionCommandTests()
    {
        _unitOfWorkStub = new();
        _sut = new(_unitOfWorkStub.Object);
    }

    [Fact]
    public async Task Handle_ShouldDelete_WhenFoundQuestion()
    {
        // Arrange
        var qId = 10;
        var pId = Guid.NewGuid();
        
        Question q = new()
        {
            Id = qId,
            ProfileId = pId
        };

        var cmd = new DeleteQuestionCommand(q.Id, q.ProfileId);

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntityAsync(q.Id)).ReturnsAsync(q);

        _unitOfWorkStub.Setup(x => x.QuestionRepository.DeleteAsync(q.Id)).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Act
        await _sut.Handle(cmd, default);

        // Assert
        _unitOfWorkStub.Verify(x => x.QuestionRepository.DeleteAsync(q.Id), Times.Once);
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowEntityNotFoundException_WhenNotFoundQuestion()
    {
        // Arrange
        var qId = 10;
        var cmd = new DeleteQuestionCommand(qId, Guid.NewGuid());

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntityAsync(qId)).ReturnsAsync((Question?)null);

        _unitOfWorkStub.Setup(x => x.QuestionRepository.DeleteAsync(qId)).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>( async () => await _sut.Handle(cmd, default));

        _unitOfWorkStub.Verify(x => x.QuestionRepository.DeleteAsync(qId), Times.Never);
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowForbiddenException_WhenQuestionIsNotFromUser()
    {
        // Arrange
        var pId = Guid.NewGuid();
        var qId = 10;

        var cmd = new DeleteQuestionCommand(qId, pId);

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntityAsync(qId)).ReturnsAsync(new Question() { Id = qId, ProfileId = Guid.NewGuid() });

        _unitOfWorkStub.Setup(x => x.QuestionRepository.DeleteAsync(qId)).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Assert
        await Assert.ThrowsAsync<ForbiddenException>(async () => await _sut.Handle(cmd, default));

        _unitOfWorkStub.Verify(x => x.QuestionRepository.DeleteAsync(qId), Times.Never);
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}