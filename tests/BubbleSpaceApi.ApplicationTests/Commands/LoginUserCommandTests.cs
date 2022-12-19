using AutoFixture;
using BubbleSpaceApi.Application.Commands.LoginUserCommand;
using BubbleSpaceApi.Application.Common;
using BubbleSpaceApi.Core.Communication.Mediator;
using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
using Moq;
using Xunit;

namespace BubbleSpaceApi.ApplicationTests.Commands;

public class LoginUserCommandTests
{
    private readonly Fixture _fixture;

    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly Mock<IMediatorHandler> _mediatorHandler;

    private readonly LoginUserCommandHandler _sut;

    public LoginUserCommandTests()
    {
        _fixture = new();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(x => _fixture.Behaviors.Remove(x));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkStub = new();
        _mediatorHandler = new();

        _sut = new(_unitOfWorkStub.Object, _mediatorHandler.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProfileId_WhenValidCredentials()
    {
        // Arrange
        var cmd = _fixture.Build<LoginUserCommand>().With(x => x.UsernameOrEmail,  "email@email.com").With(x => x.Password, "1234").Create();

        var account = _fixture.Build<Account>().With(x => x.PasswordHash, PasswordHashing.GeneratePasswordHash(cmd.Password)).Create();
        var profile = _fixture.Build<Profile>().With(x => x.AccountId, account.Id).With(x => x.Account, account).Create();

        _unitOfWorkStub.Setup(x => x.AccountRepository.GetByEmailAsync(cmd.UsernameOrEmail)).ReturnsAsync((Account?)null);
        _unitOfWorkStub.Setup(x => x.ProfileRepository.GetByUsernameAsync(cmd.UsernameOrEmail)).ReturnsAsync(profile);

        // Act
        var result = await _sut.Handle(cmd, default);

        // Assert
        Assert.Equal(profile.Id, result);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyGuid_WhenInvalidCredentials()
    {
        // Arrange
        var cmd = _fixture.Build<LoginUserCommand>().With(x => x.UsernameOrEmail,  "email@email.com").With(x => x.Password, "1234").Create();

        var account = _fixture.Build<Account>().With(x => x.PasswordHash, PasswordHashing.GeneratePasswordHash("anotherPassword!")).Create();
        var profile = _fixture.Build<Profile>().With(x => x.AccountId, account.Id).With(x => x.Account, account).Create();

        _unitOfWorkStub.Setup(x => x.AccountRepository.GetByEmailAsync(cmd.UsernameOrEmail)).ReturnsAsync((Account?)null);
        _unitOfWorkStub.Setup(x => x.ProfileRepository.GetByUsernameAsync(cmd.UsernameOrEmail)).ReturnsAsync(profile);

        // Act
        var result = await _sut.Handle(cmd, default);

        // Assert
        Assert.NotEqual(profile.Id, result);
        Assert.True(Guid.Empty == result);
    }
}