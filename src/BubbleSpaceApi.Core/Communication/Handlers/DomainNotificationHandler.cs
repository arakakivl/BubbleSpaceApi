using BubbleSpaceApi.Core.Communication.Messages.Notifications;
using MediatR;

namespace BubbleSpaceApi.Core.Communication.Handlers;

// public class DomainNotificationHandler : INotificationHandler<DomainNotification>
// {
//     private List<DomainNotification> _notifications;
//     public IReadOnlyCollection<DomainNotification> Notifications => _notifications;

//     public DomainNotificationHandler()
//     {
//         _notifications = new();
//     }

//     public Task Handle(DomainNotification notification, CancellationToken cancellationToken)
//     {
//         _notifications.Add(notification);
//         return Task.CompletedTask;
//     }

//     public bool HasNotifications() =>
//         _notifications.Count > 0;
// }

public interface IDomainNotificationHandler : INotificationHandler<DomainNotification>
{
    public bool HasNotifications();
}

public class DomainNotificationHandler : IDomainNotificationHandler
{
    private List<DomainNotification> _notifications;
    public IReadOnlyCollection<DomainNotification> Notifications => _notifications;

    public DomainNotificationHandler()
    {
        _notifications = new();
    }

    public async Task Handle(DomainNotification notification, CancellationToken cancellationToken)
    {
        _notifications.Add(notification);
        await Task.CompletedTask;
    }
    
    public bool HasNotifications() =>
        _notifications.Count > 0;
}