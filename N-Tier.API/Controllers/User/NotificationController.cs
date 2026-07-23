using MediatR;
using N_Tier.Application.Features.User.Notifications;

namespace N_Tier.API.Controllers.User;

[Route("api/notifications")]
[Authorize]
[ApiController]
public class NotificationController(ISender sender) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllNotifications.Query query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetNotificationById.Query(id),
            cancellationToken);

        return HandleResult(result);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount(
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new UnreadCountNotifications.Query(),
            cancellationToken);

        return HandleResult(result);
    }
}