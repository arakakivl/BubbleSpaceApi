using BubbleSpaceApi.Application.Commands.LoginUserCommand;
using BubbleSpaceApi.Application.Common;
using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using Moq;
using Xunit;

namespace BubbleSpaceApi.ApplicationTests.Commands;

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
    public async Task Handle_ShouldReturnProfileId_WhenValidCredentials()
    {
        // Arrange
        var cmd = new LoginUserCommand("person", "validPassword");
        var profId = Guid.NewGuid();

        Account entity = new() { PasswordHash = PasswordHashing.GeneratePasswordHash(cmd.Password) };

        _unitOfWorkStub.Setup(x => x.AccountRepository.GetByEmailAsync(cmd.UsernameOrEmail)).ReturnsAsync((Account?)null);
        _unitOfWorkStub.Setup(x => x.ProfileRepository.GetByUsernameAsync(cmd.UsernameOrEmail)).ReturnsAsync(new Profile() { AccountId = entity.Id, Account = entity, Id = profId });

        // Act
        var result = await _sut.Handle(cmd, default);

        // Assert
        Assert.Equal(profId, result);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyGuid_WhenInvalidCredentials()
    {
        // Arrange
        var cmd = new LoginUserCommand("person", "invalidPassword");
        var profId = Guid.NewGuid();

        Account entity = new() { PasswordHash = PasswordHashing.GeneratePasswordHash("anotherPassword") };

        _unitOfWorkStub.Setup(x => x.AccountRepository.GetByEmailAsync(cmd.UsernameOrEmail)).ReturnsAsync((Account?)null);
        _unitOfWorkStub.Setup(x => x.ProfileRepository.GetByUsernameAsync(cmd.UsernameOrEmail)).ReturnsAsync(new Profile() { AccountId = entity.Id, Account = entity, Id = profId });

        // Act
        var result = await _sut.Handle(cmd, default);

        // Assert
        Assert.NotEqual(result, profId);
        Assert.True(Guid.Empty == result);
    }
}