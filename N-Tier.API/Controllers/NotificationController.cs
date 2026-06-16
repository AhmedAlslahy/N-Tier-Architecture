namespace N_Tier.API.Controllers;

[Route("api/Notifications")]
[Authorize]
[ApiController]
public class NotificationController(INotificationService notificationService) : BaseController
{
    [HttpGet("")]
    public async Task<IActionResult> GetAllByUserId(CancellationToken cancellation)
    {
        var result = await notificationService.GetAllByUserId(userId, cancellation);
        return HandleResult(result);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(int Id, CancellationToken cancellation)
    {
        var result = await notificationService.GetById(Id, userId, cancellation);
        return HandleResult(result);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> UnreadCount(CancellationToken cancellation)
    {
        var result = await notificationService.UnreadCountByUserId(userId, cancellation);
        return HandleResult(result);
    }
}