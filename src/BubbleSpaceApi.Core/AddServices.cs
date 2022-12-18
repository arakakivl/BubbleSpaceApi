using BubbleSpaceApi.Core.Communication.Handlers;
using BubbleSpaceApi.Core.Communication.Mediator;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BubbleSpaceApi.Core;

public static class AddServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection collection)
    {
        collection.AddScoped<IDomainNotificationHandler, DomainNotificationHandler>();
        collection.AddScoped<IMediatorHandler, MediatorHandler>();

        return collection;
    }
}