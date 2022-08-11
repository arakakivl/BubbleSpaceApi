using BubbleSpaceApi.Api.Auth;
using BubbleSpaceApi.Api.Controllers;
using BubbleSpaceApi.Application.Commands.LoginUserCommand;
using BubbleSpaceApi.Application.Models.InputModels.LoginUserInputModel;
using BubbleSpaceApi.Application.Models.InputModels.RegisterUserModel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BubbleSpaceApi.Tests.ApiTests;

public class AccountControllerTests
{
    private readonly Mock<ISender> _senderStub;
    private readonly Mock<IAuth> _authStub;

    private readonly AccountController _sut;

    public AccountControllerTests()
    {
        _senderStub = new();
        _authStub = new();

        _sut = new(_senderStub.Object, _authStub.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnOk_WhenExecuted()
    {
        // Arrange
        var model = new RegisterUserInputModel()
        {
            Username = "someuser",
            Email = "someemail.com",
            Password = "someuser123"
        };

        // Act
        var result = await _sut.SignUpAsync(model);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnOk_WhenValidCredentials()
    {
        // Arrange
        var model = new LoginUserInputModel()
        {
            UsernameOrEmail = "someuser",
            Password = "someuser123"
        };

        var profId = Guid.NewGuid();

        _senderStub.Setup(x => x.Send(It.IsAny<LoginUserCommand>(), default)).ReturnsAsync(profId);

        _authStub.Setup(x => x.GenerateJwtToken(profId)).Returns(Guid.NewGuid().ToString());
        _authStub.Setup(x => x.GenerateRefreshToken()).Returns(Guid.NewGuid().ToString());

        // Act
        var result = await _sut.SignInAsync(model);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnBadRequest_WhenInvalidCredentials()
    {
        // Arrange
        var model = new LoginUserInputModel()
        {
            UsernameOrEmail = "someuser",
            Password = "someuser123"
        };

        _senderStub.Setup(x => x.Send(It.IsAny<LoginUserCommand>(), default)).ReturnsAsync(Guid.Empty);

        // Act
        var result = await _sut.SignInAsync(model);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}