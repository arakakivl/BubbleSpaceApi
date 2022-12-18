using BubbleSpaceApi.Core.Communication.Messages.Notifications;

namespace BubbleSpaceApi.Core.Communication.Mediator;

public interface IMediatorHandler
{
    Task PublishDomainNotificationAsync<T>(T not) where T : DomainNotification;
}