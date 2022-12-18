using BubbleSpaceApi.Core.Communication.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BubbleSpaceApi.Api.Controllers;

[ApiController]
[Route("/[controller]")]
public class ApiControllerBase : ControllerBase 
{
    private ISender _sender = null!;
    public ISender Sender => _sender;

    private IDomainNotificationHandler _domainNotificationHandler;
    public IDomainNotificationHandler DomainNotificationHandler => _domainNotificationHandler;

    public ApiControllerBase(ISender sender, IDomainNotificationHandler domainNotificationHandler)
    {
        _sender = sender;
        _domainNotificationHandler = domainNotificationHandler;
    }

    internal string GetAuthorizationBearerToken() =>
        HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

    internal string GetRefreshCookieToken() =>
        HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == "bsrfh").Value;

    internal bool HasNotifications() =>
        _domainNotificationHandler.HasNotifications();
}