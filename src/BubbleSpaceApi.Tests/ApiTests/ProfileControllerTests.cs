using BubbleSpaceApi.Api.Controllers;
using BubbleSpaceApi.Application.Exceptions;
using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Application.Queries.GetProfileQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BubbleSpaceApi.Tests.ApiTests;

public class ProfileControllerTests
{
    private readonly Mock<ISender> _senderStub;
    private readonly ProfileController _sut;

    public ProfileControllerTests()
    {
        _senderStub = new();
        _sut = new(_senderStub.Object);
    }

    /* 
        Some method endpoints cannot be executed if their model isn't on the required pattern
        By FluentValidations 
    */

    [Fact]
    public async Task GetByUsernameAsync_ShouldReturnOkWithProfile_WhenProfileNFound()
    {
        // Arrange
        var username = "someUser";
        _senderStub.Setup(x => x.Send(It.IsAny<GetProfileQuery>(), default)).ReturnsAsync(new ProfileViewModel() { Username = username });

        // Act
        var result = (await _sut.GetByUsernameAsync(username)) as OkObjectResult;

        // Assert
        Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(result!.Value);
        Assert.IsType<ProfileViewModel>(result.Value);
    }

    [Fact]
    public async Task GetByUsernameAsync_ShouldReturnNotFound_WhenProfileNotFound()
    {
        // Arrange
        var username = "someUser";
        _senderStub.Setup(x => x.Send(It.IsAny<GetProfileQuery>(), default)).ThrowsAsync(new EntityNotFoundException("Profile not found."));

        // Act
        var result = await _sut.GetByUsernameAsync(username);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}