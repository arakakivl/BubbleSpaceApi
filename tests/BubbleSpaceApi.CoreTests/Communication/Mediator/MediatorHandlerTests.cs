using AutoFixture;
using BubbleSpaceApi.Core.Communication.Mediator;
using BubbleSpaceApi.Core.Communication.Messages.Notifications;
using MediatR;
using Moq;
using Xunit;

namespace BubbleSpaceApi.CoreTests.Communication.Mediator;

public class MediatorHandlerTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IMediator> _mediatorStub;
    
    private readonly MediatorHandler _sut;

    public MediatorHandlerTests()
    {
        _fixture = new();
        _mediatorStub = new();

        _sut = new(_mediatorStub.Object);
    }

    [Fact]
    public async Task PublishDomainNotificationAsync_ShouldCallPublish_WhenExecuted()
    {
        // Arrange
        var notification = _fixture.Create<DomainNotification>();
        _mediatorStub.Setup(x => x.Publish(notification, default)).Verifiable();

        // Act
        await _sut.PublishDomainNotificationAsync(notification);

        // Assert
        _mediatorStub.Verify(x => x.Publish(notification, default), Times.Once);
    }
}