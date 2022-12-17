using BubbleSpaceApi.Application.Commands.AskQuestionCommand;
using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using Moq;
using Xunit;

namespace BubbleSpaceApi.ApplicationTests.Commands;

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

        _unitOfWorkStub.Setup(x => x.QuestionRepository.AddAsync(It.IsAny<Question>())).Verifiable();
        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();
    
        // Act
        var r = await _sut.Handle(cmd, default);

        // Assert
        Assert.Equal(q.ProfileId, proileId);

        _unitOfWorkStub.Verify(x => x.QuestionRepository.AddAsync(It.IsAny<Question>()), Times.Once);
        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}