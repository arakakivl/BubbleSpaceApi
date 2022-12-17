using BubbleSpaceApi.Application.Commands.AnswerQuestionCommand;
using BubbleSpaceApi.Application.Commands.DeleteAnswerCommand;
using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using Moq;
using Xunit;

namespace BubbleSpaceApi.ApplicationTests.Commands;

public class DeleteAnswerCommandTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly DeleteAnswerCommandHandler _sut;

    public DeleteAnswerCommandTests()
    {
        _unitOfWorkStub = new();
        _sut = new(_unitOfWorkStub.Object);
    }

    [Fact]
    public async Task Handle_ShouldDelete_WhenExecuted()
    {
        // Arrange
        var pId = Guid.NewGuid();
        var qId = 10;

        var question = new Question()
        {
            Id = qId,
            Answers = new List<Answer>() { new() { ProfileId = pId } }
        };

        var cmd = new DeleteAnswerCommand(qId, pId);

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
        var pId = Guid.NewGuid();
        var qId = 10;

        var question = new Question()
        {
            Id = qId,
            Answers = new List<Answer>() { new() { ProfileId = pId } }
        };

        var cmd = new DeleteAnswerCommand(qId, pId);

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
        var pId = Guid.NewGuid();
        var qId = 10;

        var question = new Question()
        {
            Id = qId,
            Answers = new List<Answer>()
        };

        var cmd = new DeleteAnswerCommand(qId, pId);

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntitiesAsync(q => q.Id == cmd.QuestionId, "Answers")).ReturnsAsync(new List<Question>() { question }.AsQueryable());

        _unitOfWorkStub.Setup(x => x.AnswerRepository.DeleteAsync(It.IsAny<Guid>())).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.Handle(cmd, default));

        _unitOfWorkStub.Verify(x => x.AnswerRepository.DeleteAsync(It.IsAny<Guid>()), Times.Never());
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Never());
    }
}