using BubbleSpaceApi.Application.Commands.LoginUserCommand;
using BubbleSpaceApi.Core.Entities;
using BubbleSpaceApi.Core.Interfaces;
using Moq;
using Xunit;

namespace BubbleSpaceApi.Tests.ApplicationTests.CommandTests;

public class LoginUserCommandTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly LoginUserCommandHandler _sut;

    public LoginUserCommandTests()
    {
        _unitOfWorkStub = new();
        _sut = new(_unitOfWorkStub.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTrue_WhenValidCredentials()
    {
        // Arrange
        Account entity = new() { PasswordHash = "somePassHash" };
        var cmd = new LoginUserCommand("person", "validPassword");

        _unitOfWorkStub.Setup(x => x.AccountRepository.GetByEmailAsync(cmd.UsernameOrEmail)).ReturnsAsync((Account?)null);
        _unitOfWorkStub.Setup(x => x.ProfileRepository.GetByUsernameAsync(cmd.UsernameOrEmail)).ReturnsAsync(new Profile() { AccountId = entity.Id, Account = entity });

        // Act
        var result = await _sut.Handle(cmd, default);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenInvalidCredentials()
    {
        // Arrange
        Account entity = new() { PasswordHash = "somePassHash" };
        var cmd = new LoginUserCommand("person", "invalidPassword");

        _unitOfWorkStub.Setup(x => x.AccountRepository.GetByEmailAsync(cmd.UsernameOrEmail)).ReturnsAsync((Account?)null);
        _unitOfWorkStub.Setup(x => x.ProfileRepository.GetByUsernameAsync(cmd.UsernameOrEmail)).ReturnsAsync(new Profile() { AccountId = entity.Id, Account = entity });

        // Act
        var result = await _sut.Handle(cmd, default);

        // Assert
        Assert.False(result);
    }
}