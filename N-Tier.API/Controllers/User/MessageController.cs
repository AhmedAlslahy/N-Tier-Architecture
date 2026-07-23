using MediatR;
using N_Tier.Application.Features.User.Messages;

namespace N_Tier.API.Controllers.User;

[Route("api/messages")]
[Authorize]
[ApiController]
public class MessageController(ISender sender) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Send(
       [FromForm] SendMessage.Command command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPatch("star/{id:int}")]
    public async Task<IActionResult> ToggleStar(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new ToggleStar.Command(id),
            cancellationToken);

        return HandleResult(result);
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll(
     [FromQuery] GetAllMessages.Query query,
     CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("get-all-sender")]
    public async Task<IActionResult> GetAllSender(
     [FromQuery] GetAllSender.Query query,
     CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount(
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new UnreadCountMessage.Query(),
            cancellationToken);

        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
    int id,
    CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetMessage.Query(id), cancellationToken);
        return HandleResult(result);
    }
}