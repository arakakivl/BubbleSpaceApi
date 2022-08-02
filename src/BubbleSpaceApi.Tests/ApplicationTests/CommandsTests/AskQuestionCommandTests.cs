using BubbleSpaceApi.Application.Commands.AskQuestionCommand;
using BubbleSpaceApi.Core.Entities;
using BubbleSpaceApi.Core.Interfaces;
using Moq;
using Xunit;

namespace BubbleSpaceApi.Tests.ApplicationTests.CommandTests;

public class AskQuestionCommandTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly AskQuestionCommandHandler _sut;

    public AskQuestionCommandTests()
    {
        _unitOfWorkStub = new();
        _sut = new(_unitOfWorkStub.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddToDatabase_WhenExecuted()
    {
        // Arrange
        var proileId = Guid.NewGuid();
        Question q = new()
        {
            ProfileId = proileId,
            Title = "What do you like to do when you're really sad?",
            Description = ""
        };

        AskQuestionCommand cmd = new(Guid.NewGuid(), q.Title, q.Description);

        _unitOfWorkStub.Setup(x => x.QuestionRepository.AddAsync(q)).ReturnsAsync(q.Id);

        _unitOfWorkStub.Setup(x => x.QuestionRepository.AddAsync(q)).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();
    
        // Act
        var r = await _sut.Handle(cmd, default);

        // Assert
        Assert.Equal(q.ProfileId, proileId);

        _unitOfWorkStub.Verify(x => x.QuestionRepository.AddAsync(q), Times.Once);
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Once);

    }
}