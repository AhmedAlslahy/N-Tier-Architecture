using static N_Tier.Application.Features.Notification.UnreadCountNotificationByUserId;
using static N_Tier.Application.Features.Notification.GetAllNotificationsByUserId;
using static N_Tier.Application.Features.Notification.GetNotificationById;

namespace N_Tier.API.Controllers;

[Route("api/Notifications")]
[Authorize]
[ApiController]
public class NotificationController(UnreadCountNotificationByUserIdHandler unreadCountByUser, GetNotificationByIdHandler getNotificationById
    , GetAllNotificationsByUserIdHandler getAllNotificationsByUserId) : BaseController
{
    [HttpGet("")]
    public async Task<IActionResult> GetAllByUserId()
    {
        var result = await getAllNotificationsByUserId.Handle(userId);
        return HandleResult(result);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(int Id)
    {
        var result = await getNotificationById.Handle(Id, userId);
        return HandleResult(result);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> UnreadCount()
    {
        var result = await unreadCountByUser.Handle(userId);
        return HandleResult(result);
    }
}