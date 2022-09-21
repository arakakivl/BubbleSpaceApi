using Moq;
using Xunit;
using BubbleSpaceApi.Application.Queries.GetProfileQuery;
using BubbleSpaceApi.Core.Interfaces;
using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Core.Exceptions;
using BubbleSpaceApi.Core.Entities;

namespace BubbleSpaceApi.ApplicationTests.Queries;

public class GetProfileQueryTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly GetProfileQueryHandler _sut;

    public GetProfileQueryTests()
    {
        _unitOfWorkStub = new();
        _sut = new(_unitOfWorkStub.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProfile_WhenExistentProfile()
    {
        // Arrange
        var username = "someUsername";
        _unitOfWorkStub.Setup(x => x.ProfileRepository.GetByUsernameAsync(username))
            .ReturnsAsync(new Profile()
            {});

        var query = new GetProfileQuery(username);

        // Act
        var result = await _sut.Handle(query, default);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ProfileViewModel>(result);
    }

    [Fact]
    public async Task Handle_ShouldThrowEntityNotFoundException_WhenProfileNotFound()
    {
        // Arrange
        var username = "someUsername";
        _unitOfWorkStub.Setup(x => x.ProfileRepository.GetByUsernameAsync(username)).ReturnsAsync((Profile?)null);

        var query = new GetProfileQuery(username);

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.Handle(query, default));
    }
}