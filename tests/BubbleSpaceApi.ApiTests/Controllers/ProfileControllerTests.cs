using BubbleSpaceApi.Api.Controllers;
using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Application.Queries.GetProfileQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using BubbleSpaceApi.Core.Communication.Handlers;

namespace BubbleSpaceApi.ApiTests;

public class ProfileControllerTests
{
    private readonly Mock<ISender> _senderStub;
    private readonly Mock<IDomainNotificationHandler> _domainNotificationHandlerStub;

    private readonly ProfileController _sut;

    public ProfileControllerTests()
    {
        _senderStub = new();
        _domainNotificationHandlerStub = new();
        
        _sut = new(_senderStub.Object, _domainNotificationHandlerStub.Object);
    }

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
        Assert.IsType<ResultViewModel>(result.Value);
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
        Assert.IsType<ResultViewModel>((result as NotFoundObjectResult)!.Value);
    }
}