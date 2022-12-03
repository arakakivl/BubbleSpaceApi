using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BubbleSpaceApi.Api.Controllers;

[ApiController]
[Route("/[controller]")]
public class ApiControllerBase : ControllerBase 
{
    private ISender _sender = null!;
    public ISender Sender => _sender;

    public ApiControllerBase(ISender sender)
    {
        _sender = sender;
    }

    internal string GetAuthorizationBearerToken() =>
        HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

    internal string GetRefreshCookieToken() =>
        HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == "bsrfh").Value;
}