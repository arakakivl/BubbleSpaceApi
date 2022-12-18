using System.Collections;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using BubbleSpaceApi.Api.Auth;
using BubbleSpaceApi.Api.Controllers;
using BubbleSpaceApi.Application.Commands.LoginUserCommand;
using BubbleSpaceApi.Application.Models.InputModels.LoginUserModel;
using BubbleSpaceApi.Application.Models.InputModels.RegisterUserModel;
using BubbleSpaceApi.Core.Communication.Handlers;
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
    private readonly Fixture _fixture;

    private readonly Mock<ISender> _senderStub;
    private readonly Mock<IAuth> _authStub;
    private readonly Mock<IDomainNotificationHandler> _domainNotificationHandlerStub;

    private readonly AccountController _sut;

    public AccountControllerTests()
    {
        _fixture = new();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(x => _fixture.Behaviors.Remove(x));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _senderStub = new();
        _authStub = new();
        _domainNotificationHandlerStub = new();

        _sut = new(_senderStub.Object, _authStub.Object, _domainNotificationHandlerStub.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        _sut.ControllerContext.HttpContext.Request.Headers.Authorization = "Bearer someSortOfJWTToken";
        _sut.ControllerContext.HttpContext.Request.Cookies.Append(new KeyValuePair<string, string>("bsrfh", "eyx984ut9433hg"));
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnOk_WhenNotAuthenticated()
    {
        // Arrange
        var model = _fixture.Build<RegisterUserInputModel>().With(x => x.Username, "username").With(x => x.Email, "email@email.com").Create();
        _authStub.Setup(x => x.IsAuthenticated(It.IsAny<string>())).Returns(false);

        // Act
        var result = await _sut.SignUpAsync(model);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnBadRequest_WhenAlreadyAuthenticated()
    {
        // Arrange
        _authStub.Setup(x => x.IsAuthenticated(It.IsAny<string>())).Returns(true);

        // Act
        var result = await _sut.SignUpAsync(_fixture.Create<RegisterUserInputModel>());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnOk_WhenValidCredentials()
    {
        // Arrange
        var model = _fixture.Build<LoginUserInputModel>().With(x => x.UsernameOrEmail, "username").With(x => x.Password, "password").Create();

        var profId = Guid.NewGuid();
        var claims = new Dictionary<string, string>() { { "ProfileId", profId.ToString() } };

        _senderStub.Setup(x => x.Send(It.IsAny<LoginUserCommand>(), default)).ReturnsAsync(profId);

        _authStub.Setup(x => x.GenerateToken(claims, false)).Returns(Guid.NewGuid().ToString());
        _authStub.Setup(x => x.GenerateToken(claims, true)).Returns(Guid.NewGuid().ToString());

        _authStub.Setup(x => x.IsAuthenticated(It.IsAny<string>())).Returns(false);

        // Act
        var result = await _sut.SignInAsync(model);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnBadRequest_WhenInvalidCredentials()
    {
        // Arrange
        var model = _fixture.Build<LoginUserInputModel>().With(x => x.UsernameOrEmail, "username").With(x => x.Password, "password").Create();

        _senderStub.Setup(x => x.Send(It.IsAny<LoginUserCommand>(), default)).ReturnsAsync(Guid.Empty);
        _authStub.Setup(x => x.IsAuthenticated(It.IsAny<string>())).Returns(false);

        // Act
        var result = await _sut.SignInAsync(model);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnBadRequest_WhenAlreadyAuthenticated()
    {
        // Arrange
        _authStub.Setup(x => x.IsAuthenticated(It.IsAny<string>())).Returns(true);

        // Act
        var result = await _sut.SignInAsync(_fixture.Create<LoginUserInputModel>());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnUnprocessableEntity_WhenInvalidModelProvided()
    {
        // Arrange
        _authStub.Setup(x => x.IsAuthenticated(It.IsAny<string>())).Returns(false);
        var model = _fixture.Build<LoginUserInputModel>().With(x => x.UsernameOrEmail, "").Create();

        // Act
        var result = await _sut.SignInAsync(model);

        // Assert
        Assert.IsType<UnprocessableEntityObjectResult>(result);
    }

    [Fact]
    public async Task RefreshAsync_ShouldReturnOk_WhenNotAuthenticated()
    {
        // Arrange
        _authStub.Setup(x => x.GetProfileIdFromToken(It.IsAny<string>())).Returns(Guid.NewGuid());
        _authStub.Setup(x => x.IsAuthenticated(It.IsAny<string>())).Returns(false);
        
        _authStub.Setup(x => x.GenerateToken(It.IsAny<Dictionary<string, string>>(), false)).Returns(Guid.NewGuid().ToString());
        _authStub.Setup(x => x.GenerateToken(It.IsAny<Dictionary<string, string>>(), true)).Returns(Guid.NewGuid().ToString());

        // Act
        var result = await _sut.RefreshAsync();

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task RefreshAsync_ShouldReturnBadRequest_WhenAlreadyAuthenticated()
    {
        // Arrange
        _authStub.Setup(x => x.IsAuthenticated(It.IsAny<string>())).Returns(true);

        // Act
        var result = await _sut.RefreshAsync();

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}