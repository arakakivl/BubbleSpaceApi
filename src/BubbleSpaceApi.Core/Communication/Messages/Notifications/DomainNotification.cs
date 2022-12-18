using MediatR;

namespace BubbleSpaceApi.Core.Communication.Messages.Notifications;

public class DomainNotification : INotification
{
    public string Message { get; private set; }
    public DomainNotification(string msg)
    {
        Message = msg;
    }
}