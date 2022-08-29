using System.Linq.Expressions;
using BubbleSpaceApi.Application.Commands.AnswerQuestionCommand;
using BubbleSpaceApi.Core.Exceptions;
using BubbleSpaceApi.Core.Entities;
using BubbleSpaceApi.Core.Interfaces;
using Moq;
using Xunit;

namespace BubbleSpaceApi.Tests.ApplicationTests.CommandTests;

public class AnswerQuestionCommandTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly AnswerQuestionCommandHandler _sut;

    public AnswerQuestionCommandTests()
    {
        _unitOfWorkStub = new();
        _sut = new(_unitOfWorkStub.Object);
    }
    
    [Fact]
    public async Task AnswerQuestion_ShouldAnswer_WhenNotAlreadyAnswered()
    {
        // Arrange
        var qId = 10;
        var pId = Guid.NewGuid();

        Question question = new()
        {
            Id = qId,
            Answers = new List<Answer>()  
        };

        AnswerQuestionCommand cmd = new(qId, pId, "some answer");

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntitiesAsync(q => q.Id == cmd.QuestionId, "Answers")).ReturnsAsync(new List<Question>() { question });

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
        var qId = 10;
        var pId = Guid.NewGuid();

        Question question = new()
        {
            Id = qId,
            Answers = new List<Answer>() { new Answer() { ProfileId = pId }}   
        };

        AnswerQuestionCommand cmd = new(qId, pId, "some answer");

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntitiesAsync(q => q.Id == cmd.QuestionId, "Answers")).ReturnsAsync(new List<Question>() { question });

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
        var qId = 10;
        AnswerQuestionCommand cmd = new(qId, Guid.NewGuid(), "some answer");

        _unitOfWorkStub.Setup(x => x.QuestionRepository.GetEntitiesAsync(q => q.Id == cmd.QuestionId, "Answers")).ReturnsAsync(new List<Question>() {  });

        _unitOfWorkStub.Setup(x => x.AnswerRepository.AddAsync(It.IsAny<Answer>())).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>( async () => await _sut.Handle(cmd, default));

        _unitOfWorkStub.Verify(x => x.AnswerRepository.AddAsync(It.IsAny<Answer>()), Times.Never);
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}