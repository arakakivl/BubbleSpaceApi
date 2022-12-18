using AutoFixture;
using BubbleSpaceApi.Core.Communication.Handlers;
using BubbleSpaceApi.Core.Communication.Messages.Notifications;
using Xunit;

namespace BubbleSpaceApi.CoreTests.Communication.Handlers;

public class DomainNotificationHandlerTests
{
    private readonly Fixture _fixture;
    private readonly DomainNotificationHandler _sut;
    
    public DomainNotificationHandlerTests()
    {
        _fixture = new();
        _sut = new();
    }

    [Fact]
    public async Task Handle_ShouldAddNotification_WhenExecuted()
    {
        // Arrange
        var notification = _fixture.Create<DomainNotification>();
        
        // Act
        await _sut.Handle(notification, default);
    
        // Assert
        Assert.True(_sut.HasNotifications());
    }

    [Fact]
    public async Task HasNotifications_ShouldReturnTrue_WhenHasNotifications()
    {
        // Arrange
        var notification = _fixture.Create<DomainNotification>();
        await _sut.Handle(notification, default);
        
        // Act
        var hasNotifications = _sut.HasNotifications();
    
        // Assert
        Assert.True(hasNotifications);
    }

    [Fact]
    public void HasNotifications_ShouldReturnFalse_WhenHasNotNotifications()
    {
        // Arrange
        
        // Act
        var hasNotifications = _sut.HasNotifications();
    
        // Assert
        Assert.False(hasNotifications);
    }
}