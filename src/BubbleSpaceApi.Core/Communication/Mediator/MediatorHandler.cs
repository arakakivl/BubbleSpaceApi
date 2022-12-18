using BubbleSpaceApi.Core.Communication.Messages.Notifications;
using MediatR;

namespace BubbleSpaceApi.Core.Communication.Mediator;

public class MediatorHandler : IMediatorHandler
{
    private readonly IMediator _mediator;
    public MediatorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task PublishDomainNotificationAsync<T>(T not) where T : DomainNotification =>
        await _mediator.Publish(not);
}