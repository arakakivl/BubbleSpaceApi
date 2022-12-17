using System.Linq.Expressions;
using BubbleSpaceApi.Application.Commands.AnswerQuestionCommand;
using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using Moq;
using Xunit;
using AutoFixture;

namespace BubbleSpaceApi.ApplicationTests.Commands;

public class AnswerQuestionCommandTests
{
    private readonly Fixture _fixture;

    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly AnswerQuestionCommandHandler _sut;

    public AnswerQuestionCommandTests()
    {
        _fixture = new();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(x => _fixture.Behaviors.Remove(x));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkStub = new();
        _sut = new(_unitOfWorkStub.Object);
    }
    
    [Fact]
    public async Task AnswerQuestion_ShouldAnswer_WhenNotAlreadyAnswered()
    {
        // Arrange
        var question = _fixture.Create<Question>();
        var cmd = _fixture.Build<AnswerQuestionCommand>().With(x => x.QuestionId, question.Id).Create();

        var questions = new List<Question>() { question }.AsQueryable();

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntitiesAsync(q => q.Id == cmd.QuestionId, "Answers"))
            .ReturnsAsync(questions);

        _unitOfWorkStub.Setup(x => x.AnswerRepository.AddAsync(It.IsAny<Answer>())).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Act
        await _sut.Handle(cmd, default);

        // Assert
        _unitOfWorkStub.Verify(x => x.AnswerRepository.AddAsync(It.IsAny<Answer>()), Times.Once);
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AnswerQuestion_ShouldThrownAlreadyAnsweredQuestion_WhenAlreadyAnswered()
    {
        // Arrange
        var pId = Guid.NewGuid();
        var answers = new List<Answer>() { { new() { ProfileId = pId } } };

        var question = _fixture.Build<Question>().With(x => x.Answers, answers).Create();
        var cmd = _fixture.Build<AnswerQuestionCommand>().With(x => x.QuestionId, question.Id).With(x => x.ProfileId, pId).Create();

        var questions = new List<Question>() { question }.AsQueryable();

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntitiesAsync(q => q.Id == cmd.QuestionId, "Answers"))
            .ReturnsAsync(questions);

        _unitOfWorkStub.Setup(x => x.AnswerRepository.AddAsync(It.IsAny<Answer>())).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Assert
        await Assert.ThrowsAsync<AlreadyAnsweredQuestionException>(async () => await _sut.Handle(cmd, default));

        _unitOfWorkStub.Verify(x => x.AnswerRepository.AddAsync(It.IsAny<Answer>()), Times.Never);
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task AnswerQuestion_ShouldThrownEntityNotFoundException__WhenInexistentQuestion()
    {
        // Arrange
        var cmd = _fixture.Create<AnswerQuestionCommand>();
        var questions = _fixture.CreateMany<Question>(0).AsQueryable();

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntitiesAsync(q => q.Id == cmd.QuestionId, "Answers"))
            .ReturnsAsync(questions);

        _unitOfWorkStub.Setup(x => x.AnswerRepository.AddAsync(It.IsAny<Answer>())).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>( async () => await _sut.Handle(cmd, default));

        _unitOfWorkStub.Verify(x => x.AnswerRepository.AddAsync(It.IsAny<Answer>()), Times.Never);
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}