using Moq;
using Xunit;
using BubbleSpaceApi.Application.Queries.GetProfileQuery;
using BubbleSpaceApi.Domain.Interfaces;
using BubbleSpaceApi.Application.Models.ViewModels;
using BubbleSpaceApi.Domain.Exceptions;
using BubbleSpaceApi.Domain.Entities;
using AutoFixture;

namespace BubbleSpaceApi.ApplicationTests.Queries;

public class GetProfileQueryTests
{
    private readonly Fixture _fixture;

    private readonly Mock<IUnitOfWork> _unitOfWorkStub;
    private readonly GetProfileQueryHandler _sut;

    public GetProfileQueryTests()
    {
        _fixture = new();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(x => _fixture.Behaviors.Remove(x));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWorkStub = new();
        _sut = new(_unitOfWorkStub.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProfile_WhenExistentProfile()
    {
        // Arrange
        var query = _fixture.Create<GetProfileQuery>();
        var profile = _fixture.Build<Profile>().With(x => x.Username, query.Username).Create();

        _unitOfWorkStub.Setup(x => x.ProfileRepository.GetByUsernameAsync(query.Username)).ReturnsAsync(profile);

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
        var query = _fixture.Create<GetProfileQuery>();
        _unitOfWorkStub.Setup(x => x.ProfileRepository.GetByUsernameAsync(query.Username)).ReturnsAsync((Profile?)null);

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.Handle(query, default));
    }
}