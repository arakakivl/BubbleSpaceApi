using AutoFixture;
using BubbleSpaceApi.Application.Commands.RegisterUserCommand;
using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using Moq;
using Xunit;

namespace BubbleSpaceApi.ApplicationTests.Commands;

public class RegisterUserCommandTests
{
    private readonly Fixture _fixture;

    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly RegisterUserCommandHandler _sut;

    public RegisterUserCommandTests()
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
        var cmd = _fixture.Build<RegisterUserCommand>().With(x => x.Email, "email@email.com").Create();

        _unitOfWorkStub.Setup(x => x.AccountRepository.GetByEmailAsync(cmd.Email)).ReturnsAsync((Account?)null);
        _unitOfWorkStub.Setup(x => x.AccountRepository.GetByEmailAsync(cmd.Username)).ReturnsAsync((Account?)null);

        _unitOfWorkStub.Setup(x => x.AccountRepository.AddAsync(It.IsAny<Account>())).Verifiable();
        _unitOfWorkStub.Setup(x => x.ProfileRepository.AddAsync(It.IsAny<Profile>())).Verifiable();

        _unitOfWorkStub.Setup(x => x.SaveChangesAsync()).Verifiable();

        // Act
        await _sut.Handle(cmd, default);

        // Assert
        _unitOfWorkStub.Verify(x => x.AccountRepository.AddAsync(It.IsAny<Account>()), Times.Once);
        _unitOfWorkStub.Verify(x => x.ProfileRepository.AddAsync(It.IsAny<Profile>()), Times.Once);

        _unitOfWorkStub.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}