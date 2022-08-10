using BubbleSpaceApi.Api.Controllers;
using BubbleSpaceApi.Application.Commands.LoginUserCommand;
using BubbleSpaceApi.Application.Models.InputModels.LoginUserInputModel;
using BubbleSpaceApi.Application.Models.InputModels.RegisterUserModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BubbleSpaceApi.Tests.ApiTests;

public class AccountControllerTests
{
    private readonly Mock<ISender> _senderStub;
    private readonly AccountController _sut;

    public AccountControllerTests()
    {
        _senderStub = new();
        _sut = new(_senderStub.Object);
    }

    /* 
        Some method endpoints cannot be executed if their model isn't on the required pattern
        By FluentValidations 
    */

    [Fact]
    public async Task RegisterAsync_ShouldReturnOk_IfExecuted()
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

        _senderStub.Setup(x => x.Send(It.IsAny<LoginUserCommand>(), default)).ReturnsAsync(Guid.NewGuid());

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