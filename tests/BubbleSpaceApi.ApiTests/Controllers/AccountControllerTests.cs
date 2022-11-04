using System.Collections;
using System.Diagnostics.CodeAnalysis;
using BubbleSpaceApi.Api.Auth;
using BubbleSpaceApi.Api.Controllers;
using BubbleSpaceApi.Application.Commands.LoginUserCommand;
using BubbleSpaceApi.Application.Models.InputModels.LoginUserInputModel;
using BubbleSpaceApi.Application.Models.InputModels.RegisterUserModel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace BubbleSpaceApi.ApiTests;

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

        _sut.ControllerContext.HttpContext.Request.Headers.Authorization = "Bearer someSortOfJWTToken";
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
        var claims = new Dictionary<string, string>() { { "ProfileId", profId.ToString() } };

        _senderStub.Setup(x => x.Send(It.IsAny<LoginUserCommand>(), default)).ReturnsAsync(profId);

        _authStub.Setup(x => x.GenerateToken(claims, false)).Returns(Guid.NewGuid().ToString());
        _authStub.Setup(x => x.GenerateToken(claims, true)).Returns(Guid.NewGuid().ToString());

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