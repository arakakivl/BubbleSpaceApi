using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BubbleSpaceApi.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class BaseSenderController : ControllerBase
{
    private ISender _sender = null!;
    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}