using BubbleSpaceApi.Application.Commands.RegisterUserCommand;
using BubbleSpaceApi.Core.Entities;
using BubbleSpaceApi.Core.Interfaces;
using Moq;
using Xunit;

namespace BubbleSpaceApi.Tests.ApplicationTests.CommandTests;

public class RegisterUserCommandTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly RegisterUserCommandHandler _sut;

    public RegisterUserCommandTests()
    {
        _unitOfWorkStub = new();
        _sut = new(_unitOfWorkStub.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddToDatabase_WhenExecuted()
    {
        // Should we remember that some validations (e.g., email already in use or not)
        // are  in our Validators. Then, there's no need to check if an email is already or not in use:
        // our command will not travel to the handler if the email address is already in use.

        // Arrange
        RegisterUserCommand cmd = new("person", "person@gmaill.com", "personPswd");

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